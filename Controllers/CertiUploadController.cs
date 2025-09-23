using Azure.Storage.Blobs;
using FumicertiApi.Controllers;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CertiuploadController : BaseController
{
    private readonly BlobContainerClient _containerClient;

    public CertiuploadController(IConfiguration configuration)
    {
        string connectionString = configuration["AzureStorage:ConnectionString"];
        string containerName = configuration["AzureStorage:ContainerName"];

        _containerClient = new BlobContainerClient(connectionString, containerName);
        _containerClient.CreateIfNotExists();
    }

    [HttpPost("upload-pdf-file")]
    public async Task<IActionResult> UploadPdfFile(IFormFile file, string certificateId, string certType, int companyId)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is missing");

        if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
            return BadRequest("Only PDF supported.");

        // 👇 Always override from BaseController (claims/user context)
        companyId = GetCompanyId();

        string blobUrl;
        try
        {
            blobUrl = await UploadCertificateAsync(file, certificateId, certType, companyId);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Upload error: " + ex.Message);
        }

        return Ok(new { url = blobUrl });
    }

    [NonAction]
    public async Task<string> UploadCertificateAsync(IFormFile file, string certificateId, string certType, int companyId)
    {
        try
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".pdf")
                throw new InvalidOperationException("Only PDF files are allowed.");

            var safeCertType = string.Concat(certType.Split(Path.GetInvalidFileNameChars())).Replace(" ", "_");

            // ✅ int ko string me convert karke safe bana do
            var safeCompanyId = companyId.ToString();

            // Original filename without extension
            var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
            var safeFileName = string.Concat(originalFileName.Split(Path.GetInvalidFileNameChars())).Replace(" ", "_");

            // New blob name with extension
            var blobName = $"certificates/{safeCompanyId}/{safeCertType}/{safeFileName}_{certificateId}{extension}";

            var blobClient = _containerClient.GetBlobClient(blobName);
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception($"Azure Blob upload failed: {ex.Message}", ex);
        }
    }
}
