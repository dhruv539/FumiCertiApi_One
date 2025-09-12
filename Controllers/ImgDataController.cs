using Azure.Storage.Blobs;
using FumicertiApi.Data;
using FumicertiApi.DTOs.imgdata;
using FumicertiApi.DTOs.YourApp.DTOs;
using FumicertiApi.Models;
using FumicertiApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Sieve.Models;
using Sieve.Services;
using System.Text;

namespace FumicertiApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ImgDataController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly AzureBlobService _blobService;
        public ImgDataController(AppDbContext context, ISieveProcessor sieveProcessor, AzureBlobService blobService )
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
            _blobService = blobService;
        }
        [HttpGet("report/html")]
        public async Task<IActionResult> GetImageReportHtml([FromQuery] string? user)
        {
            var query = FilterByCompany(_context.ImgData.AsQueryable(), "ImgDataCompanyId");

            if (!string.IsNullOrWhiteSpace(user))
                query = query.Where(i => i.ImgDataCreateUid == user);

            var reportData = await query
                .Where(i => !string.IsNullOrEmpty(i.ImgDataExtractedText))
                .ToListAsync();

            var groupedImages = reportData
                .GroupBy(i => string.IsNullOrWhiteSpace(i.ImgDataLocation) ? "Unknown" : i.ImgDataLocation);

            var rowsHtml = new StringBuilder();

            foreach (var group in groupedImages)
            {
                var uniqueExtractedTexts = group
                    .Select(i => i.ImgDataExtractedText)
                    .Distinct()
                    .ToList();

                string extractedTextCombined = string.Join(", ", uniqueExtractedTexts);

                rowsHtml.AppendLine($@"
        <tr>
            <td>{group.First().ImgDataCreateUid}</td>
            <td>{group.Key}</td>
            <td>{extractedTextCombined}</td>
        </tr>");
            }

            string htmlContent = $@"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <title>OCR Report</title>
    <style>
        body {{ font-family: Arial; margin: 40px; }}
        table {{ border-collapse: collapse; width: 100%; }}
        th, td {{ border: 1px solid #000; padding: 8px; text-align: left; }}
        th {{ background-color: #f0f0f0; }}
    </style>
</head>
<body onload=""window.print()"">
    <h2 style=""text-align:center;"">OCR Report</h2>
    <table>
        <thead>
            <tr>
                <th>User</th>
                <th>Location</th>
                <th>Extracted Texts</th>
            </tr>
        </thead>
        <tbody>
            {rowsHtml}
        </tbody>
    </table>
</body>
</html>";

            return Content(htmlContent, "text/html");
        }


        [HttpGet("report")]
        public async Task<IActionResult> GetImageReport()
        {
            var data = await FilterByCompany( _context.ImgData, "ImgDataCompanyId").ToListAsync();

            var report = data
                .GroupBy(x => x.ImgDataLocation)
                .Select(g => new ImgDataReportDto
                {
                    ImgDataLocation = g.Key,
                    ImgDataCreateUid = string.Join(", ", g.Select(x => x.ImgDataCreateUid).Distinct()),
                    ImgDataExtractedText = string.Join(", ", g.Select(x => x.ImgDataExtractedText))
                })
                .ToList();

            return Ok(report);
        }



        [HttpGet("list")]
        public async Task<IActionResult> GetAllWithPagination(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? location = null,
    [FromQuery] string? uploadedBy = null
)
        {
            var query = FilterByCompany(_context.ImgData.AsQueryable(), "ImgDataCompanyId");

            // Filters
            if (!string.IsNullOrEmpty(location))
                query = query.Where(x => x.ImgDataLocation != null && x.ImgDataLocation.Contains(location));

            if (!string.IsNullOrEmpty(uploadedBy))
                query = query.Where(x => x.ImgDataUserUploaded != null && x.ImgDataUserUploaded.Contains(uploadedBy));

            var totalRecords = await query.CountAsync();

            // Pagination
            var records = await query
                .OrderByDescending(x => x.ImgDataId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ImgDataReadDto
                {
                    ImgDataId = x.ImgDataId,
                    ImgData_Img_Url = x.ImgData_Img_Url!,
                    ImgDataLocation = x.ImgDataLocation,
                    ImgDataExtractedText = x.ImgDataExtractedText,
                    ImgDataUserUploaded = x.ImgDataUserUploaded,
                    ImgDataTimedate = x.ImgDataTimedate
                })
                .ToListAsync();

            return Ok(new
            {
                //message = "✅ Paginated data fetched.",
                page,
                pageSize,
                totalRecords,
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                data = records
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] SieveModel sieveModel, [FromQuery] string? user)
        {
            var currentPage = sieveModel.Page ?? 1;
            var pageSize = sieveModel.PageSize ?? 10;

            var query = FilterByCompany(_context.ImgData.AsNoTracking(), "ImgDataCompanyId");
            if (!string.IsNullOrEmpty(user))
            {
                query = query.Where(x => x.ImgDataCreateUid == user);
            }
            var filteredQuery = _sieveProcessor.Apply(sieveModel, query, applyPagination: false);
            var totalRecords = await filteredQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            var rawData = await filteredQuery
     .OrderByDescending(x => x.ImgDataId)
     .Skip((currentPage - 1) * pageSize)
     .Take(pageSize)
     .ToListAsync();

            var pagedResult = rawData.Select(x => new ImgDataReadDto
            {
                ImgDataId = x.ImgDataId,
                ImgData_Img_Url = x.ImgData_Img_Url,
                ImgDataLocation = x.ImgDataLocation,
                ImgDataTimedate = x.ImgDataTimedate,
                ImgDataExtractedText = x.ImgDataExtractedText,
                ImgDataUserUploaded = x.ImgDataUserUploaded,
                ImgDataCreated = x.ImgDataCreated,
                ImgDataUpdated = x.ImgDataUpdated,
                ImgDataCreateUid = x.ImgDataCreateUid,
                ImgDataEditedUid = x.ImgDataEditedUid,
                ImgDataCompanyId = x.ImgDataCompanyId
            }).ToList();


            return Ok(new
            {
                pagination = new
                {
                    page = currentPage,
                    pageSize = pageSize,
                    totalRecords = totalRecords,
                    totalPages = totalPages
                },
                data = pagedResult
            });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var img = await FilterByCompany(_context.ImgData.AsNoTracking(), "ImgDataCompanyId").FirstOrDefaultAsync(b => b.ImgDataId == id);
            if (img == null) return NotFound("❌ Image not found.");
            return Ok(img);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Console.WriteLine($"[API] Delete Request for ID: {id}");
            var img =  await FilterByCompany(_context.ImgData, "ImgDataCompanyId").FirstOrDefaultAsync(i => i.ImgDataId == id);
            Console.WriteLine(img != null? $"[API] Found ImgData with ID: {id}": $"[API] ImgData with ID: {id} NOT FOUND");
            if (img == null) return NotFound("❌ Image not found.");
            if (!string.IsNullOrEmpty(img.ImgData_Img_Url))
                await _blobService.DeleteAsync(img.ImgData_Img_Url);
            _context.ImgData.Remove(img);
            await _context.SaveChangesAsync();

            return Ok(true);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditImage(int id, [FromForm] ImgUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var record = await _context.ImgData.FindAsync(id);
            if (record == null)
                return NotFound("❌ Record not found.");

           
            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                // Delete old blob if exists
                if (!string.IsNullOrEmpty(record.ImgData_Img_Url))
                    await _blobService.DeleteAsync(record.ImgData_Img_Url);

                // Upload new blob with same ImageId-based filename
                var newUrl = await _blobService.UploadAsync(dto.ImageFile, id);
                record.ImgData_Img_Url = newUrl;
            }

            // Update other fields
            record.ImgDataLocation = dto.Location;
            record.ImgDataExtractedText = dto.ExtractedText;
            record.ImgDataEditedUid = GetUserId().ToString();
            record.ImgDataUpdated = DateTime.Now;
            record.ImgDataCompanyId = GetCompanyId();

            await _context.SaveChangesAsync();

            return Ok(new { message = "✅ Image updated successfully.", id = record.ImgDataId });
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] ImgUploadDto dto)
        {
            if (dto.ImageFile == null || dto.ImageFile.Length == 0)
                return BadRequest("❌ No image uploaded.");
        

            // Convert to Base64 string
            //using var ms = new MemoryStream();
            //await dto.ImageFile.CopyToAsync(ms);
            //var imageBytes = ms.ToArray();


            //var base64String = Convert.ToBase64String(imageBytes);

            var record = new ImgData
            {
                ImgData_Img_Url = null,
                // ImgDataImg = imageBytes,
                ImgDataLocation = dto.Location,
                ImgDataTimedate = DateTime.Now,
                ImgDataExtractedText = dto.ExtractedText,
                ImgDataUserUploaded = GetUserId(),
                ImgDataCreateUid = GetUserId().ToString(),
                ImgDataCreated = DateTime.Now,
                ImgDataCompanyId = GetCompanyId(),
                ImgDataUpdated = DateTime.Now
            };

            try
            {
                _context.ImgData.Add(record);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ DB Save Failed: " + ex.Message);
                Console.WriteLine("🔍 StackTrace: " + ex.StackTrace);
                return StatusCode(500, $"❌ DB Error: {ex.Message}");
            }
            // Step 2: Upload to Azure with the generated ID
            string blobUrl;
            try
            {
                blobUrl = await _blobService.UploadAsync(dto.ImageFile, record.ImgDataId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Azure Upload Failed: {ex.Message}");
            }

            // Step 3: Update DB with blob URL
            try
            {
                record.ImgData_Img_Url = blobUrl;
                _context.ImgData.Update(record);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ DB Update Failed: {ex.Message}");
            }

            return Ok(new { message = "✅ Image (base64) uploaded to Azure successfully .", id = record.ImgDataId, url = blobUrl });
        }

    }
}
