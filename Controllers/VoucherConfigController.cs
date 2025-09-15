using FumicertiApi.Data;
using FumicertiApi.DTOs;
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
        public async Task<ActionResult<IEnumerable<VoucherConfigDto>>> GetVoucherConfigs()
        {
            // return await FilterByCompany(_context.VoucherConfigs, "VoucherConfig_CompnayId").ToListAsync();

            return await FilterByCompany(_context.VoucherConfigs, "VoucherConfig_CompnayId")
        .Join(_context.branches,
              v => v.VoucherConfig_Branch_Id,
              b => b.BranchId,
              (v, b) => new VoucherConfigDto
              {
                  VoucherConfig_Id = v.VoucherConfig_Id,
                  VoucherConfig_FinYear_Id = v.VoucherConfig_FinYear_Id,
                  VoucherConfig_Branch_Id = v.VoucherConfig_Branch_Id,
                  BranchName = b.BranchName, // mapped
                  VoucherConfig_ProdType = v.VoucherConfig_ProdType,
                  VoucherConfig_Prefix = v.VoucherConfig_Prefix,
                  VoucherConfig_Suffix = v.VoucherConfig_Suffix,
                  VoucherConfig_VoucherDigit = v.VoucherConfig_VoucherDigit,
                  VoucherConfig_LastVoucherNo = v.VoucherConfig_LastVoucherNo,
                  VoucherConfig_Remarks = v.VoucherConfig_Remarks,
                  VoucherConfig_CompanyId = v.VoucherConfig_CompnayId,
                  VoucherConfig_IsLock = v.VoucherConfig_IsLock,
                  VoucherConfig_Phyto = v.VoucherConfig_Phyto
              })
        .ToListAsync();
        }

        // GET: api/voucherconfig/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VoucherConfig>> GetVoucherConfig(int id)
        {
            var config = await FilterByCompany(_context.VoucherConfigs, "VoucherConfig_CompnayId").FirstOrDefaultAsync(b => b.VoucherConfig_Id == id); 
            if (config == null) return NotFound();
            return config;
        }

        // POST: api/voucherconfig
        [HttpPost]
        public async Task<ActionResult<VoucherConfig>> CreateVoucherConfig(VoucherConfig config)
        {
            config.VoucherConfig_CompnayId = GetCompanyId();
            bool exists = await _context.VoucherConfigs
      .AnyAsync(b => b.VoucherConfig_Branch_Id == config.VoucherConfig_Branch_Id && b.VoucherConfig_Phyto == config.VoucherConfig_Phyto 
      && b.VoucherConfig_ProdType == config .VoucherConfig_ProdType && b.VoucherConfig_CompnayId == GetCompanyId());

            if (exists)
            {
                return Conflict(new { message = "A Location with the same name already exists." });
            }
            _context.VoucherConfigs.Add(config);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVoucherConfig), new { id = config.VoucherConfig_Id }, config);
        }

        // PUT: api/voucherconfig/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVoucherConfig(int id, VoucherConfig config)
        {
            if (id != config.VoucherConfig_Id) return BadRequest();
            config.VoucherConfig_CompnayId = GetCompanyId();
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
            var config = await FilterByCompany(_context.VoucherConfigs, "VoucherConfig_CompnayId").FirstOrDefaultAsync(b => b.VoucherConfig_Id == id);
            if (config == null) return NotFound();

            _context.VoucherConfigs.Remove(config);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
