using Azure.Storage.Blobs;

namespace api_ecommerce_v1.Services
{
    public interface IProductBlobConfiguration
    {
        /*
         *     Métodos que debe tener la clase que implemente esta interfaz.
         *     
         *     Nota: Todos los métodos deben ser implementados en la clase que implemente esta interfaz para evitar
         *     algún error de implementación.
        */

        //Método para eliminar un blob de un contenedor
        string DeleteBlob(string blobName, string containerName);

        //Método para obtener un blob de un contenedor
        BlobContainerClient GetBlobContainerClient(string containerName);

        //Método para subir un archivo a un contenedor
        Task<string> UploadFileBlob(IFormFile file, string containerName);
    }
}
