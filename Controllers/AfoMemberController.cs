using FumicertiApi.Data;
using FumicertiApi.DTOs;
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
    public class AfoMemberController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly ISieveProcessor _sieveProcessor;
        public AfoMemberController(AppDbContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] SieveModel sieveModel)
        {
            var currentPage = sieveModel.Page ?? 1;
            var pageSize = sieveModel.PageSize ?? 10;

            var query = FilterByCompany(_context.afo_members
                .AsNoTracking(), "AfoCompanyId"); // you can include relationships if needed

            // Apply filtering/sorting using Sieve
            var filteredQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false);

            var totalRecords = await filteredQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Apply pagination manually
            var pagedData = await filteredQuery
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new AfoMemberDto
                {
                    AfoId = m.AfoId,
                    AfoName = m.AfoName,
                    AfoMbrNo = m.AfoMbrNo,
                    AfoAlpNo = m.AfoAlpNo,
                     AfoCompanyId = m.AfoCompanyId
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
                data = pagedData
            });
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<AfoMemberDto>> Get(int id)
        {
            var member = await FilterByCompany(_context.afo_members.AsNoTracking(), "AfoCompanyId").FirstOrDefaultAsync(b => b.AfoId ==id);
            if (member == null) return NotFound();

            return Ok(new AfoMemberDto
            {
                AfoId = member.AfoId,
                AfoName = member.AfoName,
                AfoMbrNo = member.AfoMbrNo,
                AfoAlpNo = member.AfoAlpNo,
                AfoCompanyId = member.AfoCompanyId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(AfoMemberAddDto dto)
        {
            var userId = GetUserId().ToString(); // from BaseController

            var entity = new AfoMember
            {               
                AfoName = dto.AfoName,
                AfoMbrNo = dto.AfoMbrNo,
                AfoAlpNo = dto.AfoAlpNo,
                AfoAddBy = userId,
                AfoCreated = DateTime.UtcNow,
                AfoUpdated = DateTime.UtcNow,
                AfoCompanyId = GetCompanyId()
            };

            _context.afo_members.Add(entity);
            await _context.SaveChangesAsync();
            return Ok(new { entity.AfoId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AfoMemberDto dto)
        {
            var userId = GetUserId().ToString(); // from BaseController

            var member = await _context.afo_members.FindAsync(id);
            if (member == null) return NotFound();

            member.AfoName = dto.AfoName;
            member.AfoMbrNo = dto.AfoMbrNo;
            member.AfoAlpNo = dto.AfoAlpNo;
            member.AfoEditBy = userId;
            member.AfoUpdated = DateTime.UtcNow;
            member.AfoCompanyId = GetCompanyId();

            await _context.SaveChangesAsync();
            return Ok(new { success = true });

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var member = await FilterByCompany(_context.afo_members.AsNoTracking(), "AfoCompanyId").FirstOrDefaultAsync(b => b.AfoId == id);
            if (member == null) return NotFound();

            _context.afo_members.Remove(member);
            await _context.SaveChangesAsync();
            return Ok(true); // Simple boolean response

        }
    }
}
