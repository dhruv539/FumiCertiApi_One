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
    }
}
