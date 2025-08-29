using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using FumicertiApi.Models;
using FumicertiApi.Data;

namespace FumicertiApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class LocationController : BaseController
    {
        private readonly AppDbContext _context;

        public LocationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Location
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var locations = await _context.Locations.ToListAsync();
            return Ok(locations);
        }

        // GET: api/Location/type/Port
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            var filtered = await _context.Locations
                .Where(l => l.LocationType != null && l.LocationType.ToLower() == type.ToLower())
                .ToListAsync();

            if (filtered.Count == 0)
                return NotFound($"No locations found for type: {type}");

            return Ok(filtered);
        }

        // GET: api/Location/3
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
                return NotFound();

            return Ok(location);
        }

        // POST: api/Location
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Location model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();

            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;
            model.CreatedBy = userId;
            model.CompanyId = GetCompanyId();

            _context.Locations.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.LocationId }, model);
        }

        // PUT: api/Location/3
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Location model)
        {
            if (id != model.LocationId)
                return BadRequest("ID mismatch.");

            var existing = await _context.Locations.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.LocationType = model.LocationType;
            existing.LocationName = model.LocationName;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.EditedBy = GetUserId();
            existing.CompanyId = GetCompanyId();
            await _context.SaveChangesAsync();
            return NoContent(); 
        }

        // DELETE: api/Location/3
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
                return NotFound();

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();

            return Ok($"Deleted location with ID {id}");
        }
    }
}
