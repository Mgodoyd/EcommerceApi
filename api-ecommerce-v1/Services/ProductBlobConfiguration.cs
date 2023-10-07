using Azure.Storage.Blobs;

namespace api_ecommerce_v1.Services
{
    public class ProductBlobConfiguration : IProductBlobConfiguration
    {
        private readonly BlobServiceClient _blobServiceClient;

        /*
         *  Inyectamos el Servicio
         */
        public ProductBlobConfiguration(IConfiguration configuration)
        {
            string key = configuration["Blob:ConnectionString"];
            _blobServiceClient = new BlobServiceClient(key);
        }

        /*
         *  Metodo para obtener el contenedor
         */
        public BlobContainerClient GetBlobContainerClient(string containerName)
        {
            return _blobServiceClient.GetBlobContainerClient(containerName);
        }

        /*
         *  Metodo para subir un archivo al contenedor
         */
        public async Task<string> UploadFileBlob(IFormFile file, string containerName)
        {
            Stream stream = file.OpenReadStream();
            string fileName = file.FileName;
            BlobContainerClient containerClient = GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(stream, true);
            return fileName;
        }

        /*
         *  Método para eliminar un archivo del contenedor
         */
        public string DeleteBlob(string blobName, string containerName)
        {
            BlobContainerClient containerClient = GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            blobClient.DeleteIfExists();
            return blobName;
        }


    }
}
