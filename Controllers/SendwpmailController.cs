using Microsoft.AspNetCore.Mvc;

namespace FumicertiApi.Controllers
{
    using FumicertiApi.Data;
    using FumicertiApi.DTOs;
    using FumicertiApi.Models;
    using global::FumicertiApi.Data;
    using global::FumicertiApi.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Sieve.Models;
    using Sieve.Services;

        [Authorize]
        [ApiController]
        [Route("api/[controller]")]
        public class SendWpMailController : BaseController
        {
            private readonly AppDbContext _context;
            private readonly ISieveProcessor _sieveProcessor;

            public SendWpMailController(AppDbContext context, ISieveProcessor sieveProcessor)
            {
                _context = context;
                _sieveProcessor = sieveProcessor;
            }

            // GET all (with pagination & filtering)
            [HttpGet]
            public async Task<ActionResult> GetAll([FromQuery] SieveModel sieveModel)
            {
                var currentPage = sieveModel.Page ?? 1;
                var pageSize = sieveModel.PageSize ?? 10;

                var query = _context.SendWpMails.AsNoTracking(); // no FilterByCompany needed

                var filteredQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false);

                var totalRecords = await filteredQuery.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                var paged = await filteredQuery
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new SendWpMailDto
                    {
                        SendWpMailId = x.SendWpMailId,
                        SendWpMailUserName = x.SendWpMailUserName,
                        SendWpMailWpToken = x.SendWpMailWpToken,
                        SendWpMailBalanceToken = x.SendWpMailBalanceToken,
                        SendWpMailCompanyid = x.SendWpMailCompanyid

                    })
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
                    data = paged
                });
            }

            // GET by id
            [HttpGet("{id}")]
            public async Task<ActionResult<SendWpMailDto>> Get(int id)
            {
                var record = await _context.SendWpMails.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.SendWpMailId == id);

                if (record == null) return NotFound();

                return Ok(new SendWpMailDto
                {
                    SendWpMailId = record.SendWpMailId,
                    SendWpMailUserName = record.SendWpMailUserName,
                    SendWpMailWpToken = record.SendWpMailWpToken,
                    SendWpMailBalanceToken = record.SendWpMailBalanceToken,
                    SendWpMailCompanyid = record.SendWpMailCompanyid,
                   

                });
            }

        // GET all by logged-in company only
        [HttpGet("company")]
        public async Task<ActionResult<List<SendWpMailDto>>> GetByCompany()
        {
            var companyId = GetCompanyId(); // logged-in company

            var records = await _context.SendWpMails
                .AsNoTracking()
                .Where(x => x.SendWpMailCompanyid == companyId)
                .Select(x => new SendWpMailDto
                {
                    SendWpMailId = x.SendWpMailId,
                    SendWpMailUserName = x.SendWpMailUserName,
                    SendWpMailWpToken = x.SendWpMailWpToken,
                    SendWpMailBalanceToken = x.SendWpMailBalanceToken,
                    SendWpMailCompanyid = x.SendWpMailCompanyid
                })
                .ToListAsync();

            return Ok(records);
        }


            // POST (create new)
            [HttpPost]
            public async Task<IActionResult> Create(SendWpMailAddDto dto)
            {
                bool exists = await _context.SendWpMails
                    .AnyAsync(x => x.SendWpMailUserName == dto.SendWpMailUserName);

                if (exists)
                {
                    return Conflict(new { message = "A record with the same username already exists." });
                }

                var entity = new SendWpMail
                {
                    SendWpMailUserName = dto.SendWpMailUserName,
                    SendWpMailWpToken = dto.SendWpMailWpToken,
                    SendWpMailBalanceToken = dto.SendWpMailBalanceToken,
                    SendWpMailCompanyid= GetCompanyId()

                };

                _context.SendWpMails.Add(entity);
                await _context.SaveChangesAsync();

                return Ok(new { entity.SendWpMailId });
            }

            // PUT (update)
            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, SendWpMailDto dto)
            {
                var entity = await _context.SendWpMails.FindAsync(id);
                if (entity == null) return NotFound();

                entity.SendWpMailUserName = dto.SendWpMailUserName;
                entity.SendWpMailWpToken = dto.SendWpMailWpToken;
                entity.SendWpMailBalanceToken = dto.SendWpMailBalanceToken;
                
                await _context.SaveChangesAsync();
                return NoContent();
            }

            // DELETE
            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var entity = await _context.SendWpMails.FirstOrDefaultAsync(x => x.SendWpMailId== id);
                if (entity == null) return NotFound();

                _context.SendWpMails.Remove(entity);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    

}
