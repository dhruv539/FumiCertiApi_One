using FumicertiApi.Data;
using FumicertiApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FumicertiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentSerialsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EquipmentSerialsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EquipmentSerials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentSerial>>> GetEquipmentSerials()
        {
            return await _context.EquipmentSerials
                .OrderBy(e => e.EquipmentId)
                .ToListAsync();
        }

        // GET: api/EquipmentSerials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentSerial>> GetEquipmentSerial(int id)
        {
            var equipment = await _context.EquipmentSerials.FindAsync(id);

            if (equipment == null)
                return NotFound();

            return equipment;
        }

        // POST: api/EquipmentSerials
        [HttpPost]
        public async Task<ActionResult<EquipmentSerial>> PostEquipmentSerial(EquipmentSerial equipment)
        {
            equipment.EquipmentCreated = DateTime.UtcNow;

            _context.EquipmentSerials.Add(equipment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEquipmentSerial),
                new { id = equipment.EquipmentId }, equipment);
        }

        // PUT: api/EquipmentSerials/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipmentSerial(int id, EquipmentSerial equipment)
        {
            if (id != equipment.EquipmentId)
                return BadRequest();

            var existing = await _context.EquipmentSerials.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.EquipmentName = equipment.EquipmentName;
            existing.EquipmentSerialNo = equipment.EquipmentSerialNo;
            existing.EquipmentUpdated = DateTime.UtcNow;
            existing.EquipmentUpdatedBy = equipment.EquipmentUpdatedBy;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/EquipmentSerials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipmentSerial(int id)
        {
            var equipment = await _context.EquipmentSerials.FindAsync(id);
            if (equipment == null)
                return NotFound();

            _context.EquipmentSerials.Remove(equipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 🔹 SORTING API
        // GET: api/EquipmentSerials/sort/asc
        // GET: api/EquipmentSerials/sort/desc
        [HttpGet("sort/{order}")]
        public async Task<ActionResult<IEnumerable<EquipmentSerial>>> SortEquipmentSerials(string order)
        {
            if (order.ToLower() == "asc")
            {
                return await _context.EquipmentSerials
                    .OrderBy(e => e.EquipmentSerialNo)
                    .ToListAsync();
            }
            else if (order.ToLower() == "desc")
            {
                return await _context.EquipmentSerials
                    .OrderByDescending(e => e.EquipmentSerialNo)
                    .ToListAsync();
            }

            return BadRequest("Invalid sort order. Use 'asc' or 'desc'.");
        }
    }
}
