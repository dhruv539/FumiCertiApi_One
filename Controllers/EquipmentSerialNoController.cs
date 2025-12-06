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
            try
            {
                var items = await _context.EquipmentSerials
                    .OrderBy(e => e.EquipmentId)
                    .ToListAsync();

                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        // GET: api/EquipmentSerials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentSerial>> GetEquipmentSerial(int id)
        {
            try
            {
                var equipment = await _context.EquipmentSerials.FindAsync(id);

                if (equipment == null)
                    return NotFound(new { message = "Equipment not found." });

                return Ok(equipment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        // POST: api/EquipmentSerials
        [HttpPost]
        public async Task<ActionResult<EquipmentSerial>> PostEquipmentSerial(EquipmentSerial equipment)
        {
            try
            {
                if (equipment == null)
                    return BadRequest(new { message = "Invalid request body." });

                equipment.EquipmentCreated = DateTime.UtcNow;

                _context.EquipmentSerials.Add(equipment);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetEquipmentSerial),
                    new { id = equipment.EquipmentId },
                    equipment
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        // PUT: api/EquipmentSerials/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipmentSerial(int id, EquipmentSerial equipment)
        {
            try
            {
                if (id != equipment.EquipmentId)
                    return BadRequest(new { message = "ID mismatch." });

                var existing = await _context.EquipmentSerials.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = "Equipment not found." });

                existing.EquipmentName = equipment.EquipmentName;
                existing.EquipmentSerialNo = equipment.EquipmentSerialNo;
                existing.EquipmentUpdated = DateTime.UtcNow;
                existing.EquipmentUpdatedBy = equipment.EquipmentUpdatedBy;

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        // DELETE: api/EquipmentSerials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipmentSerial(int id)
        {
            try
            {
                var equipment = await _context.EquipmentSerials.FindAsync(id);
                if (equipment == null)
                    return NotFound(new { message = "Equipment not found." });

                _context.EquipmentSerials.Remove(equipment);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        // SORT: api/EquipmentSerials/sort/asc or /desc
        [HttpGet("sort/{order}")]
        public async Task<ActionResult<IEnumerable<EquipmentSerial>>> SortEquipmentSerials(string order)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(order))
                    return BadRequest(new { message = "Sort order is required." });

                IQueryable<EquipmentSerial> query = _context.EquipmentSerials;

                switch (order.ToLower())
                {
                    case "asc":
                        query = query.OrderBy(e => e.EquipmentSerialNo);
                        break;

                    case "desc":
                        query = query.OrderByDescending(e => e.EquipmentSerialNo);
                        break;

                    default:
                        return BadRequest(new { message = "Invalid sort order. Use 'asc' or 'desc'." });
                }

                var result = await query.ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }
    }
}
