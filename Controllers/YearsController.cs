﻿using FumicertiApi.Data;
using FumicertiApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FumicertiApi.Controllers
{
   
        [Route("api/[controller]")]
        [ApiController]
        public class YearsController : BaseController
    {
            private readonly AppDbContext _context;

            public YearsController(AppDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Year>>> GetYears()
            {
            return await FilterByCompany(_context.Years.AsNoTracking(), "YearCompanyId").ToListAsync();
        }

            [HttpGet("{id}")]
            public async Task<ActionResult<Year>> GetYear(int id)
            {
                var year = await FilterByCompany(_context.Years.AsNoTracking(), "YearCompanyId")
        .FirstOrDefaultAsync(y => y.YearId == id);
            if (year == null)
                    return NotFound();

                return year;
            }

            [HttpPost]
            public async Task<ActionResult<Year>> CreateYear(Year year)
        {
            year.YearCompanyId =GetCompanyId();
            year.YearCreated = DateTime.UtcNow;
                year.YearUpdated = DateTime.UtcNow;

            // If this is being set as default, unset all others for the same company
            if (year.YearIsDefault)
            {
                var existingDefaults = await _context.Years
                    .Where(y => y.YearCompanyId == year.YearCompanyId && y.YearIsDefault)
                    .ToListAsync();

                foreach (var y in existingDefaults)
                {
                    y.YearIsDefault = false;
                }
            }
            _context.Years.Add(year);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetYear), new { id = year.YearId }, year);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateYear(int id, Year year)
            {
                if (id != year.YearId)
                    return BadRequest();
            year.YearCompanyId = GetCompanyId();
                year.YearUpdated = DateTime.UtcNow;
            // If this is being set as default, unset all others for the same company
            if (year.YearIsDefault)
            {
                var existingDefaults = await _context.Years
                    .Where(y => y.YearCompanyId == year.YearCompanyId && y.YearId != year.YearId && y.YearIsDefault)
                    .ToListAsync();

                foreach (var y in existingDefaults)
                {
                    y.YearIsDefault = false;
                }
            }
            _context.Entry(year).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Years.Any(e => e.YearId == id))
                        return NotFound();
                    throw;
                }

                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteYear(int id)
            {
                var year = await FilterByCompany( _context.Years, "YearCompanyId").FirstOrDefaultAsync(b => b.YearId == id); ;
                if (year == null)
                    return NotFound();

                _context.Years.Remove(year);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }

