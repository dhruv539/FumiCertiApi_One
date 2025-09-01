using Microsoft.AspNetCore.Mvc;
using FumicertiApi.Data;
using FumicertiApi.DTOs;
using FumicertiApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System.Text;

namespace FumicertiApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportDataController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public ReportDataController(AppDbContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        // Your existing methods...
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] SieveModel sieveModel)
        {
            var currentPage = sieveModel.Page ?? 1;
            var pageSize = sieveModel.PageSize ?? 10;
            var query = _context.ReportDatas.AsNoTracking();
            var filteredQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false);
            var totalRecords = await filteredQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            var pagedData = await filteredQuery
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new ReportDataDto
                {
                    ReportDataId = r.ReportDataId,
                    DocType = r.DocType,
                    LayoutData = r.LayoutData,
                    FormatName = r.FormatName,
                    VchId = r.VchId,
                    CopyFormat = r.CopyFormat,
                    NextFormat = r.NextFormat,
                    CompanyId = r.CompanyId,
                    Status = r.Status
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
                data = pagedData
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReportDataDto>> Get(int id)
        {
            var record = await _context.ReportDatas.FindAsync(id);
            if (record == null) return NotFound();
            return Ok(new ReportDataDto
            {
                ReportDataId = record.ReportDataId,
                DocType = record.DocType,
                LayoutData = record.LayoutData,
                FormatName = record.FormatName,
                VchId = record.VchId,
                CopyFormat = record.CopyFormat,
                NextFormat = record.NextFormat,
                CompanyId = record.CompanyId,
                Status = record.Status
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReportDataAddDto dto)
        {
            var userId = GetUserId().ToString();
            var entity = new ReportData
            {
                DocType = dto.DocType,
                LayoutData = dto.LayoutData,
                FormatName = dto.FormatName,
                VchId = dto.VchId,
                CopyFormat = dto.CopyFormat,
                NextFormat = dto.NextFormat,
                CompanyId = GetCompanyId(),
                Status = dto.Status,
                CreateUid = userId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };
            _context.ReportDatas.Add(entity);
            await _context.SaveChangesAsync();
            return Ok(new { entity.ReportDataId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ReportDataDto dto)
        {
            var userId = GetUserId().ToString();
            var entity = await _context.ReportDatas.FindAsync(id);
            if (entity == null) return NotFound();
            entity.DocType = dto.DocType;
            entity.LayoutData = dto.LayoutData;
            entity.FormatName = dto.FormatName;
            entity.VchId = dto.VchId;
            entity.CopyFormat = dto.CopyFormat;
            entity.NextFormat = dto.NextFormat;
            entity.CompanyId = GetCompanyId();
            entity.Status = dto.Status;
            entity.EditedUid = userId;
            entity.Updated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await FilterByCompany(_context.ReportDatas.AsNoTracking(), "CompanyId")
                .FirstOrDefaultAsync(r => r.ReportDataId == id);
            if (entity == null) return NotFound();
            _context.ReportDatas.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok(true);
        }

        [HttpGet("layout/{id}")]
        public async Task<IActionResult> GetReportLayout(string id)
        {
            var reportXml = await _context.ReportDatas
                .Where(r => r.ReportDataId.ToString() == id)
                .Select(r => r.LayoutData)
                .FirstOrDefaultAsync();
            if (string.IsNullOrWhiteSpace(reportXml))
                return NotFound("Report layout not found.");
            var bytes = System.Text.Encoding.UTF8.GetBytes(reportXml);
            return File(bytes, "application/xml");
        }

        // NEW ENDPOINTS FOR REPORT DESIGNER

        [HttpGet("designer/reports")]
        [AllowAnonymous] // Remove if auth required
        public async Task<IActionResult> GetDesignerReportsList()
        {
            try
            {
                var reports = await _context.ReportDatas
                    .Where(r => r.CompanyId == GetCompanyId() && !string.IsNullOrEmpty(r.FormatName))
                    .Select(r => new { r.FormatName })
                    .Distinct()
                    .ToListAsync();

                var result = reports.ToDictionary(x => x.FormatName, x => x.FormatName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting reports list: {ex.Message}");
            }
        }

        [HttpGet("designer/reports/{reportName}")]
        [AllowAnonymous] // Remove if auth required
        public async Task<IActionResult> GetDesignerReportData(string reportName)
        {
            try
            {
                var report = await _context.ReportDatas
                    .FirstOrDefaultAsync(r => r.FormatName == reportName && r.CompanyId == GetCompanyId());

                if (report?.LayoutData == null)
                {
                    // Return empty report template
                    var emptyTemplate = @"<?xml version=""1.0"" encoding=""utf-8""?>
<XtraReportsLayoutSerializer SerializerVersion=""24.1.3.0"" Ref=""1"" ControlType=""DevExpress.XtraReports.UI.XtraReport, DevExpress.XtraReports.v24.1"" Name=""XtraReport"" PageWidth=""850"" PageHeight=""1100"" Version=""24.1"">
  <Bands>
    <Item1 Ref=""2"" ControlType=""DevExpress.XtraReports.UI.TopMarginBand, DevExpress.XtraReports.v24.1"" Name=""TopMargin"" />
    <Item2 Ref=""3"" ControlType=""DevExpress.XtraReports.UI.BottomMarginBand, DevExpress.XtraReports.v24.1"" Name=""BottomMargin"" />
    <Item3 Ref=""4"" ControlType=""DevExpress.XtraReports.UI.DetailBand, DevExpress.XtraReports.v24.1"" Name=""Detail"" />
  </Bands>
  <Version>24.1</Version>
  <DataMember>Query</DataMember>
</XtraReportsLayoutSerializer>";
                    return Content(emptyTemplate, "application/xml", Encoding.UTF8);
                }

                return Content(report.LayoutData, "application/xml", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting report data: {ex.Message}");
            }
        }

        [HttpPost("designer/reports/{reportName}")]
        [AllowAnonymous] // Remove if auth required
        public async Task<IActionResult> SaveDesignerReportData(string reportName)
        {
            try
            {
                using var reader = new StreamReader(Request.Body, Encoding.UTF8);
                var layoutData = await reader.ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(layoutData))
                {
                    return BadRequest("Layout data is empty");
                }

                var userId = GetUserId()?.ToString() ?? "system";
                var companyId = GetCompanyId();

                var existingReport = await _context.ReportDatas
                    .FirstOrDefaultAsync(r => r.FormatName == reportName && r.CompanyId == companyId);

                if (existingReport != null)
                {
                    existingReport.LayoutData = layoutData;
                    existingReport.EditedUid = userId;
                    existingReport.Updated = DateTime.UtcNow;
                }
                else
                {
                    var newReport = new ReportData
                    {
                        FormatName = reportName,
                        LayoutData = layoutData,
                        DocType = "Report",
                        CompanyId = companyId,
                        CreateUid = userId,
                        Created = DateTime.UtcNow,
                        Updated = DateTime.UtcNow,
                        Status = 1
                    };
                    _context.ReportDatas.Add(newReport);
                }

                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Report saved successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error saving report: {ex.Message}");
            }
        }

        [HttpDelete("designer/reports/{reportName}")]
        [AllowAnonymous] // Remove if auth required
        public async Task<IActionResult> DeleteDesignerReport(string reportName)
        {
            try
            {
                var report = await _context.ReportDatas
                    .FirstOrDefaultAsync(r => r.FormatName == reportName && r.CompanyId == GetCompanyId());

                if (report == null)
                    return NotFound("Report not found");

                _context.ReportDatas.Remove(report);
                await _context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting report: {ex.Message}");
            }
        }
    }
}
