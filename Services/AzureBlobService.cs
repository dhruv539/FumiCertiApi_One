using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace FumicertiApi.Services
{
    public class AzureBlobService
    {
        private readonly BlobContainerClient _containerClient;

        public AzureBlobService(IConfiguration config)
        {
            var connectionString = config["AzureStorage:ConnectionString"];
            var containerName = config["AzureStorage:ContainerName"];

            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task<string> UploadAsync(IFormFile file, int imageId)
        {
            var blobName = $"certi/ImageId{imageId}{Path.GetExtension(file.FileName)}";
            var blobClient = _containerClient.GetBlobClient(blobName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString(); // ✅ URL to save in MySQL
        }

        public async Task DeleteAsync(string blobUrl)
        {
            var uri = new Uri(blobUrl);

            // Example: "/container/certi/ImageId50.jpg"
            var segments = uri.AbsolutePath.TrimStart('/').Split('/', 2);

            if (segments.Length < 2)
                throw new InvalidOperationException("Invalid blob URL: " + blobUrl);

            var blobName = segments[1]; // "certi/ImageId50.jpg"

            await _containerClient.DeleteBlobIfExistsAsync(blobName);
        }




        /// <summary> Dhruv HAdiya
        /// Upload a PDF certificate to Azure Blob Storage
        /// </summary>
        public async Task<string> UploadCertificateAsync(IFormFile file, int certificateId)
        {
            // Ensure it's a PDF
            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                throw new InvalidOperationException("Only PDF files are allowed.");

            // Optional: unique filename to avoid collisions
            var blobName = $"certificates/CertificateId{certificateId}_{Guid.NewGuid()}.pdf";
            var blobClient = _containerClient.GetBlobClient(blobName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString(); // Save this URL in your DB
        }

        /// <summary>
        /// Delete a PDF certificate from Azure Blob Storage
        /// </summary>
        public async Task DeleteCertificateAsync(string blobUrl)
        {
            var blobClient = new BlobClient(new Uri(blobUrl));
            await blobClient.DeleteIfExistsAsync();
        }
    }
}
