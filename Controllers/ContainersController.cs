using FumicertiApi.Data;
using FumicertiApi.DTOs.Container;
using FumicertiApi.Models;
using FumicertiApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace FumicertiApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContainersController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly ISieveProcessor _sieveProcessor;
        public ContainersController(AppDbContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] SieveModel sieveModel)
        {
            var currentPage = sieveModel.Page ?? 1;
            var pageSize = sieveModel.PageSize ?? 10;

            var query =  FilterByCompany(_context.Containers.AsNoTracking(), "CotainerCompanyId");

            var filteredQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false);

            var totalRecords = await filteredQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var pagedContainers = await filteredQuery
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ContainerReadDto
                {
                    ContainerCid = c.ContainerCid,
                    ContainerCertiId = c.ContainerCertiId,
                    ContainerContainerNo = c.ContainerContainerNo,
                    ContainerCsize = c.ContainerCsize,
                    ContainerConsumeQty = c.ContainerConsumeQty,
                    ContainerDt1 = c.ContainerDt1,
                    ContainerDt2 = c.ContainerDt2,
                    ContainerDt3 = c.ContainerDt3,
                    ContainerTime1 = c.ContainerTime1,
                    ContainerTime2 = c.ContainerTime2,
                    ContainerTime3 = c.ContainerTime3,
                    ContainerFb1 = c.ContainerFb1,
                    ContainerFb2 = c.ContainerFb2,
                    ContainerFb3 = c.ContainerFb3,
                    ContainerMc1 = c.ContainerMc1,
                    ContainerMc2 = c.ContainerMc2,
                    ContainerMc3 = c.ContainerMc3,
                    ContainerTb1 = c.ContainerTb1,
                    ContainerTb2 = c.ContainerTb2,
                    ContainerTb3 = c.ContainerTb3,
                    ContainerEquilibrium = c.ContainerEquilibrium,
                    ContainerVolL = c.ContainerVolL,
                    ContainerVolB = c.ContainerVolB,
                    ContainerVolH = c.ContainerVolH,
                    ContainerProdID1 = c.ContainerProdID1,
                    ContainerProdID2 = c.ContainerProdID2,
                    ContainerProdID3 = c.ContainerProdID3,
                    ContainerQty1 = c.ContainerQty1,
                    ContainerQty2 = c.ContainerQty2,
                    ContainerWt1 = c.ContainerWt1,
                    ContainerWt2 = c.ContainerWt2,
                    ContainerWt3 = c.ContainerWt3,
                    ContainerFbper1 = c.ContainerFbper1,
                    ContainerFbper2 = c.ContainerFbper2,
                    ContainerFbper3 = c.ContainerFbper3,
                    ContainerMcper1 = c.ContainerMcper1,
                    ContainerMcper2 = c.ContainerMcper2,
                    ContainerMcper3 = c.ContainerMcper3,
                    ContainerTbper1 = c.ContainerTbper1,
                    ContainerTbper2 = c.ContainerTbper2,
                    ContainerTbper3 = c.ContainerTbper3,
                    ContainerEquipmentType = c.ContainerEquipmentType,
                    ContainerProductname = c.ContainerProductname,
                    ContainerActualDoseRate = c.ContainerActualDoseRate,
                    ContainerFirstTvl = c.ContainerFirstTvl,
                    ContainerSecondTlv = c.ContainerSecondTlv,
                    ContainerCalculateDose = c.ContainerCalculateDose,
                    ContainerCreateUid = c.ContainerCreateUid,
                    ContainerEditedUid = c.ContainerEditedUid,
                    ContainerCreated = c.ContainerCreated,
                    CotainerCompanyId = c.CotainerCompanyId,
                    ContainerUpdated = c.ContainerUpdated
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
                data = pagedContainers
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContainerReadDto>> GetById(int id)
        {
            var container =  await FilterByCompany(_context.Containers.AsNoTracking(), "CotainerCompanyId").FirstOrDefaultAsync(b => b.ContainerCid == id);
            if (container == null)
                return NotFound();

            // manual mapping from EF entity → DTO
            var dto = new ContainerReadDto
            {
                ContainerCid = container.ContainerCid,
                ContainerCertiId = container.ContainerCertiId,
                ContainerContainerNo = container.ContainerContainerNo,
                ContainerCsize = container.ContainerCsize,
                ContainerConsumeQty = container.ContainerConsumeQty, // ✅ decimal?
                ContainerDt1 = container.ContainerDt1,
                ContainerDt2 = container.ContainerDt2,
                ContainerDt3 = container.ContainerDt3,
                ContainerTime1 = container.ContainerTime1,
                ContainerTime2 = container.ContainerTime2,
                ContainerTime3 = container.ContainerTime3,
                ContainerFb1 = container.ContainerFb1,
                ContainerFb2 = container.ContainerFb2,
                ContainerFb3 = container.ContainerFb3,
                ContainerMc1 = container.ContainerMc1,
                ContainerMc2 = container.ContainerMc2,
                ContainerMc3 = container.ContainerMc3,
                ContainerTb1 = container.ContainerTb1,
                ContainerTb2 = container.ContainerTb2,
                ContainerTb3 = container.ContainerTb3,
                ContainerEquilibrium = container.ContainerEquilibrium,
                ContainerVolL = container.ContainerVolL,
                ContainerVolB = container.ContainerVolB,
                ContainerVolH = container.ContainerVolH,
                ContainerProdID1 = container.ContainerProdID1,
                ContainerProdID2 = container.ContainerProdID2,
                ContainerProdID3 = container.ContainerProdID3,
                ContainerQty1 = container.ContainerQty1,
                ContainerQty2 = container.ContainerQty2,
                ContainerWt1 = container.ContainerWt1,
                ContainerWt2 = container.ContainerWt2,
                ContainerWt3 = container.ContainerWt3,
                ContainerFbper1 = container.ContainerFbper1,
                ContainerFbper2 = container.ContainerFbper2,
                ContainerFbper3 = container.ContainerFbper3,
                ContainerMcper1 = container.ContainerMcper1,
                ContainerMcper2 = container.ContainerMcper2,
                ContainerMcper3 = container.ContainerMcper3,
                ContainerTbper1 = container.ContainerTbper1,
                ContainerTbper2 = container.ContainerTbper2,
                ContainerTbper3 = container.ContainerTbper3,
                ContainerEquipmentType = container.ContainerEquipmentType,
                ContainerProductname = container.ContainerProductname,
                ContainerActualDoseRate = container.ContainerActualDoseRate,
                ContainerFirstTvl = container.ContainerFirstTvl,
                ContainerSecondTlv = container.ContainerSecondTlv,
                ContainerCalculateDose = container.ContainerCalculateDose,
                ContainerCreateUid = container.ContainerCreateUid,
                ContainerEditedUid = container.ContainerEditedUid,
                ContainerCreated = container.ContainerCreated,
                ContainerUpdated = container.ContainerUpdated,

                // 👉 extra varchar fields you added in DB
                ContainerVolumecbm = container.ContainerVolumecbm,
                ContainerQtymbrgram = container.ContainerQtymbrgram,
                Container100Mbrgram = container.Container100Mbrgram,
                ContainerRequredprod1 = container.ContainerRequredprod1,
                ContainerRequredprod2 = container.ContainerRequredprod2,
                ContainerReqcylinder = container.ContainerReqcylinder,
                ContainerP1 = container.ContainerP1,
                ContainerP2 = container.ContainerP2,
                ContainerTotalqtygram = container.ContainerTotalqtygram,
                ContainerExcessqtygrams = container.ContainerExcessqtygrams,
                CotainerCompanyId = container.CotainerCompanyId,
                ContainerTotalqtyconsumed = container.ContainerTotalqtyconsumed
            };

            return Ok(dto);
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ContainerAddDto dto)
        {
            var model = new Container
            {
                // DO NOT assign ContainerCid if it's auto-increment
                ContainerCertiId = dto.ContainerCertiId,
                ContainerContainerNo = dto.ContainerContainerNo,
                ContainerCsize = dto.ContainerCsize,
                ContainerConsumeQty = dto.ContainerConsumeQty,
                ContainerDt1 = dto.ContainerDt1,
                ContainerDt2 = dto.ContainerDt2,
                ContainerDt3 = dto.ContainerDt3,
                ContainerTime1 = dto.ContainerTime1,
                ContainerTime2 = dto.ContainerTime2,
                ContainerTime3 = dto.ContainerTime3,
                ContainerFb1 = dto.ContainerFb1,
                ContainerFb2 = dto.ContainerFb2,
                ContainerFb3 = dto.ContainerFb3,
                ContainerMc1 = dto.ContainerMc1,
                ContainerMc2 = dto.ContainerMc2,
                ContainerMc3 = dto.ContainerMc3,
                ContainerTb1 = dto.ContainerTb1,
                ContainerTb2 = dto.ContainerTb2,
                ContainerTb3 = dto.ContainerTb3,
                ContainerEquilibrium = dto.ContainerEquilibrium,
                ContainerVolL = dto.ContainerVolL,
                ContainerVolB = dto.ContainerVolB,
                ContainerVolH = dto.ContainerVolH,
                ContainerProdID1 = dto.ContainerProdID1,
                ContainerProdID2 = dto.ContainerProdID2,
                ContainerProdID3 = dto.ContainerProdID3,
                ContainerQty1 = dto.ContainerQty1,
                ContainerQty2 = dto.ContainerQty2,
                ContainerWt1 = dto.ContainerWt1,
                ContainerWt2 = dto.ContainerWt2,
                ContainerWt3 = dto.ContainerWt3,
                ContainerFbper1 = dto.ContainerFbper1,
                ContainerFbper2 = dto.ContainerFbper2,
                ContainerFbper3 = dto.ContainerFbper3,
                ContainerMcper1 = dto.ContainerMcper1,
                ContainerMcper2 = dto.ContainerMcper2,
                ContainerMcper3 = dto.ContainerMcper3,
                ContainerTbper1 = dto.ContainerTbper1,
                ContainerTbper2 = dto.ContainerTbper2,
                ContainerTbper3 = dto.ContainerTbper3,
                ContainerEquipmentType = dto.ContainerEquipmentType,
                ContainerProductname = dto.ContainerProductname,
                ContainerActualDoseRate = dto.ContainerActualDoseRate,
                ContainerFirstTvl = dto.ContainerFirstTvl,
                ContainerSecondTlv = dto.ContainerSecondTlv,

                ContainerCalculateDose = dto.ContainerCalculateDose,
                Container100Mbrgram= dto.Container100Mbrgram,
                ContainerCid=dto.ContainerCid,
                ContainerExcessqtygrams=dto.ContainerExcessqtygrams,
                ContainerP1=dto.ContainerP1,
                ContainerP2=dto.ContainerP2,
                ContainerQtymbrgram=dto.ContainerQtymbrgram,
                ContainerReqcylinder=dto.ContainerReqcylinder,
                ContainerRequredprod1=dto.ContainerRequredprod1,
                ContainerRequredprod2=dto.ContainerRequredprod2,
                ContainerTotalqtyconsumed=dto.ContainerTotalqtyconsumed,
                ContainerTotalqtygram=dto.ContainerTotalqtygram,
                ContainerVolumecbm=dto.ContainerVolumecbm,

                ContainerCreateUid = dto.ContainerCreateUid,
                ContainerEditedUid = dto.ContainerEditedUid,
                ContainerCreated = DateTime.UtcNow,
                CotainerCompanyId = GetCompanyId(),
                ContainerUpdated = DateTime.UtcNow
            };

            _context.Containers.Add(model);
            await _context.SaveChangesAsync();

            return Ok(true);
        }


        // ✅ Flexible Search (certiId, containerNo, or both)
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ContainerReadDto>>> Search(
            [FromQuery] string? certiId,
            [FromQuery] string? containerNo)
        {
            var query = FilterByCompany( _context.Containers.AsQueryable(), "CotainerCompanyId");

            // if certiId provided → filter
            if (!string.IsNullOrEmpty(certiId))
                query = query.Where(c => c.ContainerCertiId == certiId);

            // if containerNo provided → filter
            if (!string.IsNullOrEmpty(containerNo))
                query = query.Where(c => c.ContainerContainerNo == containerNo);

            var containers = await query.ToListAsync();

            if (!containers.Any())
                return NotFound(new { message = "No containers found for the given criteria." });

            var result = containers.Select(MapToDto).ToList();
            return Ok(result);
        }
        private ContainerReadDto MapToDto(Container container)
        {
            return new ContainerReadDto
            {
                ContainerCid = container.ContainerCid,
                ContainerCertiId = container.ContainerCertiId,
                ContainerContainerNo = container.ContainerContainerNo,
                ContainerCsize = container.ContainerCsize,
                ContainerConsumeQty = container.ContainerConsumeQty, // careful: string in DTO, decimal in DB
                ContainerDt1 = container.ContainerDt1,
                ContainerDt2 = container.ContainerDt2,
                ContainerDt3 = container.ContainerDt3,
                ContainerTime1 = container.ContainerTime1,
                ContainerTime2 = container.ContainerTime2,
                ContainerTime3 = container.ContainerTime3,
                ContainerFb1 = container.ContainerFb1,
                ContainerFb2 = container.ContainerFb2,
                ContainerFb3 = container.ContainerFb3,
                ContainerMc1 = container.ContainerMc1,
                ContainerMc2 = container.ContainerMc2,
                ContainerMc3 = container.ContainerMc3,
                ContainerTb1 = container.ContainerTb1,
                ContainerTb2 = container.ContainerTb2,
                ContainerTb3 = container.ContainerTb3,
                ContainerEquilibrium = container.ContainerEquilibrium,
                ContainerVolL = container.ContainerVolL,
                ContainerVolB = container.ContainerVolB,
                ContainerVolH = container.ContainerVolH,
                ContainerProdID1 = container.ContainerProdID1,
                ContainerProdID2 = container.ContainerProdID2,
                ContainerProdID3 = container.ContainerProdID3,
                ContainerQty1 = container.ContainerQty1,
                ContainerQty2 = container.ContainerQty2,
                ContainerWt1 = container.ContainerWt1,
                ContainerWt2 = container.ContainerWt2,
                ContainerWt3 = container.ContainerWt3,
                ContainerFbper1 = container.ContainerFbper1,
                ContainerFbper2 = container.ContainerFbper2,
                ContainerFbper3 = container.ContainerFbper3,
                ContainerMcper1 = container.ContainerMcper1,
                ContainerMcper2 = container.ContainerMcper2,
                ContainerMcper3 = container.ContainerMcper3,
                ContainerTbper1 = container.ContainerTbper1,
                ContainerTbper2 = container.ContainerTbper2,
                ContainerTbper3 = container.ContainerTbper3,
                ContainerEquipmentType = container.ContainerEquipmentType,
                ContainerProductname = container.ContainerProductname,
                ContainerActualDoseRate = container.ContainerActualDoseRate,
                ContainerFirstTvl = container.ContainerFirstTvl,
                ContainerSecondTlv = container.ContainerSecondTlv,
                ContainerCalculateDose = container.ContainerCalculateDose,
                ContainerCreateUid = container.ContainerCreateUid,
                ContainerEditedUid = container.ContainerEditedUid,
                ContainerCreated = container.ContainerCreated,
                ContainerUpdated = container.ContainerUpdated
            };
        }

        // PUT: api/Containers/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ContainerAddDto dto)
        {
            var model = await _context.Containers.FindAsync(id);
            if (model == null)
                return NotFound();

            model.ContainerCertiId = dto.ContainerCertiId;
            model.ContainerContainerNo = dto.ContainerContainerNo;
            model.ContainerCsize = dto.ContainerCsize;
            model.ContainerConsumeQty = dto.ContainerConsumeQty;
            model.ContainerDt1 = dto.ContainerDt1;
            model.ContainerDt2 = dto.ContainerDt2;
            model.ContainerDt3 = dto.ContainerDt3;
            model.ContainerTime1 = dto.ContainerTime1;
            model.ContainerTime2 = dto.ContainerTime2;
            model.ContainerTime3 = dto.ContainerTime3;
            model.ContainerFb1 = dto.ContainerFb1;
            model.ContainerFb2 = dto.ContainerFb2;
            model.ContainerFb3 = dto.ContainerFb3;
            model.ContainerMc1 = dto.ContainerMc1;
            model.ContainerMc2 = dto.ContainerMc2;
            model.ContainerMc3 = dto.ContainerMc3;
            model.ContainerTb1 = dto.ContainerTb1;
            model.ContainerTb2 = dto.ContainerTb2;
            model.ContainerTb3 = dto.ContainerTb3;
            model.ContainerEquilibrium = dto.ContainerEquilibrium;
            model.ContainerVolL = dto.ContainerVolL;
            model.ContainerVolB = dto.ContainerVolB;
            model.ContainerVolH = dto.ContainerVolH;
            model.ContainerProdID1 = dto.ContainerProdID1;
            model.ContainerProdID2 = dto.ContainerProdID2;
            model.ContainerProdID3 = dto.ContainerProdID3;
            model.ContainerQty1 = dto.ContainerQty1;
            model.ContainerQty2 = dto.ContainerQty2;
            model.ContainerWt1 = dto.ContainerWt1;
            model.ContainerWt2 = dto.ContainerWt2;
            model.ContainerWt3 = dto.ContainerWt3;
            model.ContainerFbper1 = dto.ContainerFbper1;
            model.ContainerFbper2 = dto.ContainerFbper2;
            model.ContainerFbper3 = dto.ContainerFbper3;
            model.ContainerMcper1 = dto.ContainerMcper1;
            model.ContainerMcper2 = dto.ContainerMcper2;
            model.ContainerMcper3 = dto.ContainerMcper3;
            model.ContainerTbper1 = dto.ContainerTbper1;
            model.ContainerTbper2 = dto.ContainerTbper2;
            model.ContainerTbper3 = dto.ContainerTbper3;
            model.ContainerEquipmentType = dto.ContainerEquipmentType;
            model.ContainerProductname = dto.ContainerProductname;
            model.ContainerActualDoseRate = dto.ContainerActualDoseRate;
            model.ContainerFirstTvl = dto.ContainerFirstTvl;
            model.ContainerSecondTlv = dto.ContainerSecondTlv;

            model.ContainerCalculateDose = dto.ContainerCalculateDose;
            model.Container100Mbrgram = dto.Container100Mbrgram;
            model.ContainerCid = dto.ContainerCid;
            model.ContainerExcessqtygrams = dto.ContainerExcessqtygrams;
            model.ContainerP1 = dto.ContainerP1;
            model.ContainerP2 = dto.ContainerP2;
            model.ContainerQty1 = dto.ContainerQty1;
            model.ContainerQty2 = dto.ContainerQty2;
            model.ContainerQtymbrgram = dto.ContainerQtymbrgram;
            model.ContainerReqcylinder = dto.ContainerReqcylinder;
            model.ContainerRequredprod1 = dto.ContainerRequredprod1;
            model.ContainerRequredprod2 = dto.ContainerRequredprod2;
            model.ContainerVolumecbm = dto.ContainerVolumecbm;
            model.ContainerTotalqtygram = dto.ContainerTotalqtygram;

            model.CotainerCompanyId = GetCompanyId();



            model.ContainerEditedUid = dto.ContainerEditedUid;
            model.ContainerUpdated = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Update failed. The container may have been modified or deleted.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var container = await FilterByCompany(_context.Containers.AsNoTracking(), "CotainerCompanyId").FirstOrDefaultAsync(b => b.ContainerCid == id);
            if (container == null) return NotFound();

            _context.Containers.Remove(container);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("report")]
        public async Task<ActionResult> GetCertiContainerReport(
            [FromQuery] DateTime fromdate,
            [FromQuery] DateTime todate,
            [FromQuery] string? certiNo,
            [FromQuery] string? containerNo,
            [FromQuery] string? certiType,
            [FromServices] ReportService reportService)
        {
            var result = await reportService.GetCertiContainerReportAsync(fromdate, todate, certiNo, containerNo, certiType);

            if (result == null || !result.Any())
                return NotFound(new { message = "No records found for given filters." });

            return Ok(new
            {
                filter = new { fromdate, todate, certiNo, containerNo, certiType },
                count = result.Count,
                data = result
            });
        }



    }
}
