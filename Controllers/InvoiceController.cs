using FumicertiApi.Data;
using FumicertiApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace FumicertiApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public InvoiceController(AppDbContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SieveModel sieveModel)
        {
            var currentPage = sieveModel.Page ?? 1;
            var pageSize = sieveModel.PageSize ?? 10;

            var query = _context.Invoices.AsNoTracking();
            var filteredQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false);

            var totalRecords = await filteredQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var data = await filteredQuery
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                pagination = new
                {
                    page = currentPage,
                    pageSize,
                    totalRecords,
                    totalPages
                },
                data
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.InvoiceDetails) // Include details
                .FirstOrDefaultAsync(i => i.InvId == id);

            if (invoice == null) return NotFound();
            return Ok(invoice);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Invoice dto)
        {
            dto.InvCreated = DateTime.UtcNow;
            dto.InvCreateUid = GetUserId().ToString();

            var details = dto.InvoiceDetails?.ToList();
            dto.InvoiceDetails = null;

            _context.Invoices.Add(dto);
            await _context.SaveChangesAsync();

            if (details != null && details.Count > 0)
            {
                foreach (var detail in details)
                {
                    detail.InvoiceDetailId = null;
                    detail.InvoiceDetailInvoiceId = dto.InvId;
                    detail.InvoiceDetailCreated = DateTime.UtcNow;
                    detail.InvoiceDetailCreateUid = GetUserId().ToString();
                    detail.InvoiceDetailCompanyId = GetCompanyId();
                    _context.InvoiceDetails.Add(detail);
                }

                await _context.SaveChangesAsync();
            }

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Invoice dto)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null) return NotFound();

            _context.Entry(invoice).CurrentValues.SetValues(dto);
            invoice.InvUpdated = DateTime.UtcNow;
            invoice.InvEditedUid = GetUserId().ToString();
            invoice.InvCompanyId = GetCompanyId();
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.InvoiceDetails)
                .FirstOrDefaultAsync(i => i.InvId == id);

            if (invoice == null) return NotFound();

            if (invoice.InvoiceDetails?.Any() == true)
                _context.InvoiceDetails.RemoveRange(invoice.InvoiceDetails);

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
