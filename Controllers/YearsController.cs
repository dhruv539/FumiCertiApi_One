using FumicertiApi.Data;
using FumicertiApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FumicertiApi.Controllers
{
   
        [Route("api/[controller]")]
        [ApiController]
        public class YearsController : ControllerBase
        {
            private readonly AppDbContext _context;

            public YearsController(AppDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Year>>> GetYears()
            {
                return await _context.Years.ToListAsync();
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Year>> GetYear(int id)
            {
                var year = await _context.Years.FindAsync(id);
                if (year == null)
                    return NotFound();

                return year;
            }

            [HttpPost]
            public async Task<ActionResult<Year>> CreateYear(Year year)
            {
                year.YearCreated = DateTime.UtcNow;
                year.YearUpdated = DateTime.UtcNow;

                _context.Years.Add(year);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetYear), new { id = year.YearId }, year);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateYear(int id, Year year)
            {
                if (id != year.YearId)
                    return BadRequest();

                year.YearUpdated = DateTime.UtcNow;
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
                var year = await _context.Years.FindAsync(id);
                if (year == null)
                    return NotFound();

                _context.Years.Remove(year);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }

