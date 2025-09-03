using FumicertiApi.Data;
using FumicertiApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FumicertiApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoucherConfigController : BaseController
    {
        private readonly AppDbContext _context;

        public VoucherConfigController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/voucherconfig
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VoucherConfig>>> GetVoucherConfigs()
        {
            return await _context.VoucherConfigs.ToListAsync();
        }

        // GET: api/voucherconfig/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VoucherConfig>> GetVoucherConfig(int id)
        {
            var config = await _context.VoucherConfigs.FindAsync(id);
            if (config == null) return NotFound();
            return config;
        }

        // POST: api/voucherconfig
        [HttpPost]
        public async Task<ActionResult<VoucherConfig>> CreateVoucherConfig(VoucherConfig config)
        {
            _context.VoucherConfigs.Add(config);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVoucherConfig), new { id = config.VoucherConfig_Id }, config);
        }

        // PUT: api/voucherconfig/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVoucherConfig(int id, VoucherConfig config)
        {
            if (id != config.VoucherConfig_Id) return BadRequest();

            _context.Entry(config).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.VoucherConfigs.Any(e => e.VoucherConfig_Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/voucherconfig/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoucherConfig(int id)
        {
            var config = await _context.VoucherConfigs.FindAsync(id);
            if (config == null) return NotFound();

            _context.VoucherConfigs.Remove(config);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
