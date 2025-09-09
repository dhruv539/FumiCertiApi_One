using FumicertiApi.Data;
using FumicertiApi.Interface;
using FumicertiApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FumicertiApi.Services
{
    public class ConsumptionReportService : IConsumptionReportService
    {
        private readonly AppDbContext _context;

        public ConsumptionReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetConsumptionReportAsync(DateTime from, DateTime to, string type, int companyId)
        {
            ValidateInputs(from, to, type);

            // Certificates for the specified period
            var certis = await _context.Certi.AsNoTracking()
                .Where(c => c.CertiFumidate >= from && c.CertiFumidate <= to &&
                            c.CertiProductType == type &&
                            c.CertiCompanyId == companyId)
                .ToListAsync();

            if (!certis.Any())
                return new { message = "No certificates found in given date range." };

            // Related Data
            var relatedData = await LoadRelatedDataAsync(certis, from, to, companyId, type);

            // Certificates result
            var certificateList = BuildCertificateResults(certis, relatedData);

            // Invoices for the specified period
            var invoiceList = relatedData.Invoices.Select(i => new
            {
                i.InvNo,
                i.InvTotalWtKg,
                i.InvSupplierId,
                i.InvDate,
                SupplierName = i.InvSupplierId.HasValue && relatedData.SuppliersById.ContainsKey(i.InvSupplierId.Value)
                    ? relatedData.SuppliersById[i.InvSupplierId.Value].NotifyName
                    : null,
                i.Invprodtype
            }).ToList();

            // Total Consumed for the specified period
            var allContainers = relatedData.ContainersByCertiId
                .SelectMany(kvp => kvp.Value)
                .ToList();
            decimal totalConsumedKg = CalculateTotalKg(allContainers);

            // Period summary based on from-to dates
            var periodSummary = await BuildPeriodSummaryAsync(from, to, type, companyId, totalConsumedKg);

            // Return object
            return new
            {
                Certificates = certificateList,
                Invoices = invoiceList,
                PeriodSummary = periodSummary,
                TotalConsumedKg = totalConsumedKg,
                Period = new { From = from, To = to }
            };
        }

        #region Private Helpers

        private static void ValidateInputs(DateTime from, DateTime to, string type)
        {
            if (from == default || to == default)
                throw new ArgumentException("From and To date are required.");
            if (from > to)
                throw new ArgumentException("From date cannot be greater than To date.");
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Enter a Product Type.");
        }

        private async Task<(Dictionary<string, List<Container>> ContainersByCertiId,
                           Dictionary<int, Company> CompaniesById,
                           Dictionary<string, AfoMember> AfoByName,
                           List<Invoice> Invoices,
                           Dictionary<int, Notify> SuppliersById)>
        LoadRelatedDataAsync(List<Certi> certis, DateTime from, DateTime to, int companyId, string type)
        {
            var certiIds = certis.Select(c => c.CertiId).ToList();
            var companyIds = certis.Select(c => c.CertiCompanyId).Distinct().ToList();
            var afoNames = certis
                .Where(c => !string.IsNullOrWhiteSpace(c.CertiAfoName))
                .Select(c => c.CertiAfoName!)
                .Distinct()
                .ToList();

            // Get containers for certificates in the period
            var containers = await _context.Containers
                .Where(c => certiIds.Contains(c.ContainerCertiId))
                .ToListAsync();

            // Get companies
            var companies = await _context.companies
                .Where(c => companyIds.Contains(c.CompanyId) && c.Status)
                .ToListAsync();

            // Get AFO members
            var afoMembers = await _context.afo_members
                .Where(a => afoNames.Contains(a.AfoName))
                .ToListAsync();

            // Get invoices for the specified period
            var invoices = await _context.Invoices
                .Where(i => i.InvDate >= from
                            && i.InvDate <= to
                            && i.InvCompanyId == companyId
                            && i.Invprodtype == type)
                .ToListAsync();

            // Get suppliers
            var supplierIds = invoices
                .Where(i => i.InvSupplierId.HasValue)
                .Select(i => i.InvSupplierId!.Value)
                .Distinct()
                .ToList();

            var suppliers = await _context.Notifies
                .Where(n => supplierIds.Contains(n.NotifyId))
                .ToListAsync();

            // Create dictionaries
            var containersByCertiId = containers
                .GroupBy(c => c.ContainerCertiId)
                .ToDictionary(g => g.Key, g => g.ToList());
            var companiesById = companies.ToDictionary(c => c.CompanyId, c => c);
            var afoByName = afoMembers.ToDictionary(a => a.AfoName, a => a);
            var suppliersById = suppliers.ToDictionary(s => s.NotifyId, s => s);

            return (containersByCertiId, companiesById, afoByName, invoices, suppliersById);
        }

        private List<object> BuildCertificateResults(
            List<Certi> certis,
            (Dictionary<string, List<Container>> ContainersByCertiId,
             Dictionary<int, Company> CompaniesById,
             Dictionary<string, AfoMember> AfoByName,
             List<Invoice> Invoices,
             Dictionary<int, Notify> SuppliersById) data)
        {
            var results = new List<object>();

            foreach (var certi in certis)
            {
                data.ContainersByCertiId.TryGetValue(certi.CertiId, out var certiContainers);
                certiContainers ??= new List<Container>();
                decimal totalKg = CalculateTotalKg(certiContainers);

                data.CompaniesById.TryGetValue(certi.CertiCompanyId, out var company);
                data.AfoByName.TryGetValue(certi.CertiAfoName ?? "", out var afo);

                results.Add(new
                {
                    Certificate = new
                    {
                        certi.CertiNo,
                        certi.CertiFumidate,
                        certi.CertiJobType,
                        certi.CertiContainerSize,
                        certi.CertiContainerCount,
                        certi.CertiProductType,
                        Cargo = certi.CertiCargoDesc,
                        NetQty = $"{certi.CertiNetQty} {certi.CertiNetUnit}",
                        ImportCountry = certi.CertiImpcountry,
                        certi.CertiDoseRate,
                        certi.CertiRemarks,
                        TotalContainerQtyKg = totalKg
                    },
                    Company = new
                    {
                        Name = company?.Name ?? "N/A",
                        Address = string.Join(", ", new[]
                        {
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

            return results;
        }

        private static decimal CalculateTotalKg(IEnumerable<Container> containers)
        {
            decimal totalKg = 0;

            foreach (var c in containers)
            {
                decimal consumedGrams = 0;

                if (decimal.TryParse(c.ContainerTotalqtyconsumed?.Trim(), out var parsedConsumed))
                {
                    consumedGrams = parsedConsumed;
                }
                else if (decimal.TryParse(c.ContainerTotalqtygram?.Trim(), out var parsedGrams))
                {
                    consumedGrams = parsedGrams;
                }

                totalKg += consumedGrams / 1000; // grams → KG
            }

            return totalKg;
        }

        private async Task<object> BuildPeriodSummaryAsync(DateTime from, DateTime to, string type, int companyId, decimal totalConsumedKg)
        {
            // Get the financial year start date
            int finYearStartYear = from.Month >= 4 ? from.Year : from.Year - 1;
            DateTime finYearStart = new DateTime(finYearStartYear, 4, 1);

            // Get initial opening balance from products table
            var initialOpening = await _context.products
                .Where(p => p.ProductType == type && p.ProductCompanyId == companyId)
                .SumAsync(p => p.ProductOpening);

            // Calculate purchases before the 'from' date (from financial year start to 'from' date)
            var purchasesBeforeFromDate = await _context.Invoices
                .Where(i => i.InvDate >= finYearStart &&
                            i.InvDate < from &&
                            i.InvCompanyId == companyId &&
                            i.Invprodtype == type)
                .SumAsync(i => Convert.ToDecimal(i.InvTotalWtKg ?? 0));

            // Calculate consumption before the 'from' date
            var certiIdsBeforeFrom = await _context.Certi.AsNoTracking()
                .Where(c => c.CertiFumidate >= finYearStart &&
                            c.CertiFumidate < from &&
                            c.CertiProductType == type &&
                            c.CertiCompanyId == companyId)
                .Select(c => c.CertiId)
                .ToListAsync();

            var containersBeforeFrom = await _context.Containers
                .Where(c => certiIdsBeforeFrom.Contains(c.ContainerCertiId))
                .ToListAsync();

            decimal consumptionBeforeFromKg = CalculateTotalKg(containersBeforeFrom);

            // Opening balance as of 'from' date
            decimal openingAsOfFromDate = initialOpening + purchasesBeforeFromDate - consumptionBeforeFromKg;

            // Purchases during the specified period (from - to)
            var purchasesDuringPeriod = await _context.Invoices
                .Where(i => i.InvDate >= from &&
                            i.InvDate <= to &&
                            i.InvCompanyId == companyId &&
                            i.Invprodtype == type)
                .SumAsync(i => Convert.ToDecimal(i.InvTotalWtKg ?? 0));

            // Total available for the period
            decimal totalAvailableForPeriod = openingAsOfFromDate + purchasesDuringPeriod;

            // Remaining balance after consumption
            decimal remainingBalance = totalAvailableForPeriod - totalConsumedKg;

            return new
            {
                PeriodStart = from,
                PeriodEnd = to,
                OpeningBalanceKg = Math.Round(openingAsOfFromDate, 3),
                PurchasesDuringPeriodKg = Math.Round(purchasesDuringPeriod, 3),
                TotalAvailableKg = Math.Round(totalAvailableForPeriod, 3),
                TotalConsumedKg = Math.Round(totalConsumedKg, 3),
                RemainingBalanceKg = Math.Round(remainingBalance, 3),

                // Additional breakdown for clarity
                Breakdown = new
                {
                    InitialOpening = Math.Round(initialOpening, 3),
                    PurchasesBeforeFromDate = Math.Round(purchasesBeforeFromDate, 3),
                    ConsumptionBeforeFromDate = Math.Round(consumptionBeforeFromKg, 3)
                }
            };
        }

        #endregion
    }
}
