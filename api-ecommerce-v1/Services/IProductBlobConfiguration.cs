using Azure.Storage.Blobs;

namespace api_ecommerce_v1.Services
{
    public interface IProductBlobConfiguration
    {
        string DeleteBlob(string blobName, string containerName);
        BlobContainerClient GetBlobContainerClient(string containerName);
        Task<string> UploadFileBlob(IFormFile file, string containerName);
    }
}
