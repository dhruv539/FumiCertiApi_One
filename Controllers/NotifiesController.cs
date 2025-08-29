using FumicertiApi.Data;
using FumicertiApi.DTOs.Notify;
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

        public class NotifyController : BaseController
        {
        private readonly AppDbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public NotifyController(AppDbContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SieveModel sieveModel)
        {
            var query = _context.Notifies.AsQueryable();

            // Apply filtering/sorting/pagination via Sieve
            var filteredQuery = _sieveProcessor.Apply(sieveModel, query);

            var totalRecords = await query.CountAsync(); // Full count (before pagination)
            var paginatedList = await filteredQuery.ToListAsync(); // Paged result

            // Optional: Map to DTO
            var notifies = paginatedList.Select(n => new NotifyReadDto
            {
                NotifyId=n.NotifyId,
                NotifyName = n.NotifyName,
                NotifyEmail = n.NotifyEmail,
                NotifyType = n.NotifyType,
                NotifyStatus = n.NotifyStatus,
                NotifyCompanyId = n.NotifyCompanyId,
                NotifyAddress = n.NotifyAddress,
                NotifyContactNo = n.NotifyContactNo,
                NotifyCreated = n.NotifyCreated,
                NotifyUpdated = n.NotifyUpdated,
                NotifyState = n.NotifyState,
                NotifyPincode = n.NotifyPincode,
                NotifyGstNo = n.NotifyGstNo,
                NotifyStateCode = n.NotifyStateCode,
                NotifyCountry = n.NotifyCountry,
                NotifyStateId = n.NotifyStateId,
                NotifyCreateUid = GetUserId().ToString(),
                NotifyEditedUid = GetUserId().ToString(),
               
            }).ToList();

            var result = new
            {
                pagination = new
                {
                    page = (sieveModel.Page ?? 1),
                    pageSize = (sieveModel.PageSize ?? 10),
                    totalRecords
                },
                data = notifies
            };

            return Ok(result);
        }



        // GET: api/Notifies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotifyReadDto>> GetById(int id)
        {
            var notify = await _context.Notifies.FindAsync(id);
            if (notify == null) return NotFound();

            var dto = new NotifyReadDto
            {
                NotifyId = notify.NotifyId,
                NotifyName = notify.NotifyName,
                NotifyEmail = notify.NotifyEmail,
                NotifyType = notify.NotifyType,
                NotifyStatus = notify.NotifyStatus,
                NotifyCompanyId = notify.NotifyCompanyId,
                NotifyAddress = notify.NotifyAddress,
                NotifyContactNo = notify.NotifyContactNo,
                NotifyCreated = notify.NotifyCreated,
                NotifyUpdated = notify.NotifyUpdated,
                NotifyState = notify.NotifyState,
                NotifyPincode = notify.NotifyPincode,
                NotifyGstNo = notify.NotifyGstNo,
                NotifyStateCode = notify.NotifyStateCode,
                NotifyCountry = notify.NotifyCountry,
                NotifyStateId = notify.NotifyStateId,
                NotifyCreateUid = GetUserId().ToString(),
                NotifyEditedUid = GetUserId().ToString(),
            };

            return Ok(dto);
        }

        // POST: api/Notifies
        [HttpPost]
        public async Task<ActionResult<NotifyReadDto>> Create([FromBody] NotifyAddDto dto)
        {
            var notify = new Notify
            {
                NotifyCompanyId = GetCompanyId(),     // 🔒 Backend-only
                NotifyCreateUid = GetUserId().ToString(),        // 🔒 Backend-only
                NotifyCreated = DateTime.UtcNow,
                NotifyUpdated = DateTime.UtcNow,

                // DTO values
                NotifyName = dto.NotifyName,
                NotifyEmail = dto.NotifyEmail,
                NotifyType = dto.NotifyType,
                NotifyStatus = dto.NotifyStatus,
                NotifyAddress = dto.NotifyAddress,
                NotifyContactNo = dto.NotifyContactNo,
                NotifyGstNo = dto.NotifyGstNo,
                NotifyState = dto.NotifyState,
                NotifyPincode = dto.NotifyPincode,
                NotifyStateCode = dto.NotifyStateCode,
                NotifyCountry = dto.NotifyCountry,
                NotifyStateId = dto.NotifyStateId
            };

            _context.Notifies.Add(notify);
            await _context.SaveChangesAsync();

            // Create DTO to return
            var readDto = new NotifyReadDto
            {
                NotifyId = notify.NotifyId,
                NotifyName = notify.NotifyName,
                NotifyEmail = notify.NotifyEmail,
                NotifyType = notify.NotifyType,
                NotifyStatus = notify.NotifyStatus,
                NotifyAddress = notify.NotifyAddress,
                NotifyContactNo = notify.NotifyContactNo,
                NotifyGstNo = notify.NotifyGstNo,
                NotifyState = notify.NotifyState,
                NotifyPincode = notify.NotifyPincode,
                NotifyStateCode = notify.NotifyStateCode,
                NotifyCountry = notify.NotifyCountry,
                NotifyStateId = notify.NotifyStateId
            };

            return CreatedAtAction(nameof(GetById), new { id = notify.NotifyId }, readDto);
        }


        // PUT: api/Notifies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, NotifyUpdateDto dto)
        {
            if (id != dto.NotifyId)
                return BadRequest();

            var notify = await _context.Notifies.FindAsync(id);
            if (notify == null) return NotFound();

            notify.NotifyName = dto.NotifyName;
            notify.NotifyEmail = dto.NotifyEmail;
            notify.NotifyType = dto.NotifyType;
            notify.NotifyStatus = dto.NotifyStatus;
            notify.NotifyCompanyId = GetCompanyId();
            notify.NotifyAddress = dto.NotifyAddress;
            notify.NotifyContactNo = dto.NotifyContactNo;
            notify.NotifyGstNo = dto.NotifyGstNo;
            notify.NotifyState = dto.NotifyState;
            notify.NotifyPincode = dto.NotifyPincode;
            notify.NotifyStateCode = dto.NotifyStateCode;
            notify.NotifyCountry = dto.NotifyCountry;
            notify.NotifyStateId = dto.NotifyStateId;
            notify.NotifyEditedUid = GetUserId().ToString();
            notify.NotifyUpdated = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Notifies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var notify = await _context.Notifies.FindAsync(id);
            if (notify == null) return NotFound();

            _context.Notifies.Remove(notify);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Notify with ID {id} has been deleted." });
        }

        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            var companyId = GetCompanyId();

            // Log incoming type and companyId for debug
            Console.WriteLine($"🔍 Input type: {type}, CompanyId: {companyId}");

            var list = await _context.Notifies
                .Where(n =>
                    !string.IsNullOrEmpty(n.NotifyType) &&
                    n.NotifyType.ToLower() == type.ToLower() &&
                    n.NotifyCompanyId == companyId
                )
                .Select(n => new Notify
                {
                    NotifyId = n.NotifyId,
                    NotifyName = n.NotifyName,
                    NotifyEmail = n.NotifyEmail,
                    NotifyType = n.NotifyType,
                    NotifyStatus = n.NotifyStatus,
                    NotifyCompanyId = n.NotifyCompanyId,
                    NotifyAddress = n.NotifyAddress,
                    NotifyContactNo = n.NotifyContactNo,                 
                    NotifyGstNo = n.NotifyGstNo,                
                   NotifyCountry = n.NotifyCountry,                 
                })
                .ToListAsync();

            // Log count for debug
            Console.WriteLine($"📦 Found: {list.Count} records");

            return Ok(list);
        }



    }
}
