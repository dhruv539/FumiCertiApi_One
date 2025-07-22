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
    public class BranchController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly ISieveProcessor _sieveProcessor;
        public BranchController(AppDbContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] SieveModel sieveModel)
        {
            var currentPage = sieveModel.Page ?? 1;
            var pageSize = sieveModel.PageSize ?? 10;

            var query = _context.branches.AsNoTracking();

            var filteredQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false);

            var totalRecords = await filteredQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var pagedBranches = await filteredQuery
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BranchDto
                {
                    BranchId = b.BranchId,
                    BranchName = b.BranchName,
                    Gstin = b.Gstin,
                    Pan = b.Pan,
                    Address1 = b.Address1,
                    City = b.City,
                    State = b.State,
                    Country = b.Country,
                    Email = b.Email,
                    ContactNo = b.ContactNo,
                    Status = b.Status
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
                data = pagedBranches
            });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BranchDto>> Get(int id)
        {
            var branch = await _context.branches.FindAsync(id);
            if (branch == null) return NotFound();

            return Ok(new BranchDto
            {
                BranchId = branch.BranchId,
                BranchName = branch.BranchName,
                Gstin = branch.Gstin,
                Pan = branch.Pan,
                Address1 = branch.Address1,
                City = branch.City,
                State = branch.State,
                Country = branch.Country,
                Email = branch.Email,
                ContactNo = branch.ContactNo,
                Status = branch.Status
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(BranchAddDto dto)
        {
            var branch = new Branch
            {
                BranchName = dto.BranchName,
                Gstin = dto.Gstin,
                Pan = dto.Pan,
                Address1 = dto.Address1,
                City = dto.City,
                State = dto.State,
                Country = dto.Country,
                Email = dto.Email,
                ContactNo = dto.ContactNo,
                Status = dto.Status,
                Created = DateTime.UtcNow,
                AddBy = GetUserId().ToString(),
            };

            _context.branches.Add(branch);
            await _context.SaveChangesAsync();

            return Ok(new { branch.BranchId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BranchDto dto)
        {
            var branch = await _context.branches.FindAsync(id);
            if (branch == null) return NotFound();

            branch.BranchName = dto.BranchName;
            branch.Gstin = dto.Gstin;
            branch.Pan = dto.Pan;
            branch.Address1 = dto.Address1;
            branch.City = dto.City;
            branch.State = dto.State;
            branch.Country = dto.Country;
            branch.Email = dto.Email;
            branch.ContactNo = dto.ContactNo;
            branch.Status = dto.Status;
            branch.Updated = DateTime.UtcNow;
            branch.UpdateBy = GetUserId().ToString();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var branch = await _context.branches.FindAsync(id);
            if (branch == null) return NotFound();

            _context.branches.Remove(branch);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
