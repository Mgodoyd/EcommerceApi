using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface IConfig
    {
        /*
         *     Métodos que debe tener la clase que implemente esta interfaz.
         *     
         *     Nota: Todos los métodos deben ser implementados en la clase que implemente esta interfaz para evitar
         *     algún error de implementación.
        */


        // Método para crear una nueva configuración.
        Config CrearConfig(Config config);

        // Método para actualizar una configuración.
        Config ActualizarConfig(int configId, Config confiActualizado);

        // Método para eliminar una configuración.
        Config ObtenerConfyporId(int configId);
    }
}
