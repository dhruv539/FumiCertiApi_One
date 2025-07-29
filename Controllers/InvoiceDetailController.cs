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
    public class InvoiceDetailController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public InvoiceDetailController(AppDbContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SieveModel sieveModel)
        {
            try
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
                        pageSize = pageSize,
                        totalRecords = totalRecords,
                        totalPages = totalPages
                    },
                    data = data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, detail = ex.StackTrace });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var detail = await _context.InvoiceDetails.FindAsync(id);
            if (detail == null) return NotFound();
            return Ok(detail);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InvoiceDetail dto)
        {
            dto.InvoiceDetailCreated = DateTime.UtcNow;
            dto.InvoiceDetailCreateUid = GetUserId().ToString();

            _context.InvoiceDetails.Add(dto);
            await _context.SaveChangesAsync();
            return Ok(new { dto.InvoiceDetailId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, InvoiceDetail dto)
        {
            var detail = await _context.InvoiceDetails.FindAsync(id);
            if (detail == null) return NotFound();

            _context.Entry(detail).CurrentValues.SetValues(dto);
            detail.InvoiceDetailUpdated = DateTime.UtcNow;
            detail.InvoiceDetailEditedUid = GetUserId().ToString();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var detail = await _context.InvoiceDetails.FindAsync(id);
            if (detail == null) return NotFound();

            _context.InvoiceDetails.Remove(detail);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
