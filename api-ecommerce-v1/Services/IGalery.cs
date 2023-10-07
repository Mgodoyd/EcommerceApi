using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface IGalery
    {
        /*
         *     Métodos que debe tener la clase que implemente esta interfaz.
         *     
         *     Nota: Todos los métodos deben ser implementados en la clase que implemente esta interfaz para evitar
         *     algún error de implementación.
        */

        // Método para obtener toda la galeria por id de producto
        List<Galery> ObtenerGaleryPorProductId(int productId);

        // Método para crear la galeria
        Galery CrearGalery(Galery galery);

        // Método para actualizar toda la galeria
        Galery ActualizarGalery(int productId, Galery galeryActualizado);

        // Método para eliminar toda la galeria
        bool EliminarGalery(int galeryId);
    }
}
