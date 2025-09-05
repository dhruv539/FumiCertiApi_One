using FumicertiApi.Data;
using FumicertiApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FumicertiApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ConsumptionReportDataPrintController : BaseController
    {
        private readonly AppDbContext _context;

        public ConsumptionReportDataPrintController(AppDbContext context)
        {
            _context = context;
        }

        // Example: GET /api/ConsumptionReportDataPrint?from=2025-09-01&to=2025-09-05
        [HttpGet]
        public async Task<ActionResult> GetConsumptionReport([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (from == default || to == default)
                return BadRequest(new { message = "From and To date are required." });

            if (from > to)
                return BadRequest(new { message = "From date cannot be greater than To date." });

            // Step 1: Get certi list between dates
            var certis = await FilterByCompany(_context.Certi.AsNoTracking(), "CertiCompanyId")
                .Where(c => c.CertiDate >= from && c.CertiDate <= to)
                .ToListAsync();

            if (!certis.Any())
                return NotFound(new { message = "No certificates found in given date range." });

            var certiIds = certis.Select(c => c.CertiId).ToList();
            var companyIds = certis.Select(c => c.CertiCompanyId).Distinct().ToList();
            var afoNames = certis.Select(c => c.CertiAfoName).Where(n => n != null).Distinct().ToList();

            // Step 2: Batch load related data
            var containers = await _context.Containers
                .Where(x => certiIds.Contains(x.ContainerCertiId))
                .ToListAsync();

            var companies = await _context.companies
                .Where(c => companyIds.Contains(c.CompanyId) && c.Status == true)
                .ToListAsync();

            var afoMembers = await _context.afo_members
                .Where(a => afoNames.Contains(a.AfoName))
                .ToListAsync();

            var invoices = await _context.Invoices
                .Where(i => i.InvDate >= from && i.InvDate <= to && i.InvCompanyId == GetCompanyId())
                .ToListAsync();

            var supplierIds = invoices.Select(i => i.InvSupplierId).Where(id => id.HasValue).Select(id => id.Value).Distinct().ToList();
            var suppliers = await _context.Notifies
                .Where(n => supplierIds.Contains(n.NotifyId))
                .ToListAsync();

            // Step 3: Build lookup dictionaries
            var containersByCertiId = containers.GroupBy(c => c.ContainerCertiId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var companiesById = companies.ToDictionary(c => c.CompanyId, c => c);
            var afoByName = afoMembers.ToDictionary(a => a.AfoName, a => a);
            var suppliersById = suppliers.ToDictionary(s => s.NotifyId, s => s);

            // Step 4: Build certificate result
            var certificateList = new List<object>();

            foreach (var certi in certis)
            {
                // Containers
                var certiContainers = containersByCertiId.TryGetValue(certi.CertiId, out var cList) ? cList : new List<Container>();
                decimal totalGrams = 0;
                foreach (var container in certiContainers)
                {
                    if (!string.IsNullOrWhiteSpace(container.ContainerTotalqtyconsumed))
                    {
                        if (decimal.TryParse(container.ContainerTotalqtyconsumed.Trim(), out var parsedQty))
                        {
                            totalGrams += parsedQty;
                        }
                    }
                    else if (container.ContainerConsumeQty.HasValue)
                    {
                        totalGrams += container.ContainerConsumeQty.Value;
                    }
                }
                var totalKg = totalGrams / 1000;

                // Company
                companiesById.TryGetValue(certi.CertiCompanyId, out var company);

                // AFO
                AfoMember afo = null;
                if (!string.IsNullOrWhiteSpace(certi.CertiAfoName))
                    afoByName.TryGetValue(certi.CertiAfoName, out afo);

                // Add to result
                certificateList.Add(new
                {
                    Certificate = new
                    {
                        certi.CertiNo,
                        certi.CertiDate,
                        Cargo = certi.CertiCargoDesc,
                        NetQty = $"{certi.CertiNetQty} {certi.CertiNetUnit}",
                        ImportCountry = certi.CertiImpcountry,
                        certi.CertiDoseRate,
                        certi.CertiRemarks,
                        TotalContainerQtyKg = totalKg,
                        Debug = new
                        {
                            ContainerCount = certiContainers.Count,
                            RawContainerData = certiContainers.Select(c => new
                            {
                                c.ContainerTotalqtyconsumed,
                                c.ContainerConsumeQty
                            })
                        }
                    },
                    Company = new
                    {
                        Name = company?.Name ?? "N/A",
                        Address = string.Join(", ", new[] {
                            company?.Address1, company?.Address2, company?.Address3,
                            company?.City, company?.StateCode, company?.Country, company?.Pincode
                        }.Where(s => !string.IsNullOrWhiteSpace(s))),
                        Contact = company?.Mobile ?? "",
                        Email = company?.Email ?? ""
                    },
                    Afo = new
                    {
                        afo?.AfoName,
                        afo?.AfoMbrNo
                    }
                });
            }

            // Step 5: Build invoice result (for the month, not per certificate)
            var invoiceList = invoices.Select(i => new
            {
                i.InvNo,
                i.InvTotalWtKg,
                i.InvSupplierId,
                SupplierName = i.InvSupplierId.HasValue && suppliersById.ContainsKey(i.InvSupplierId.Value)
                    ? suppliersById[i.InvSupplierId.Value].NotifyName
                    : null
            }).ToList();

            // Step 6: Return both certificates and invoices
            return Ok(new
            {
                Certificates = certificateList,
                Invoices = invoiceList
            });
        }
    }
}
