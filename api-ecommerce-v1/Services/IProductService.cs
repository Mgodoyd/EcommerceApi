using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface IProductService
    {
        /*
         *     Métodos que debe tener la clase que implemente esta interfaz.
         *     
         *     Nota: Todos los métodos deben ser implementados en la clase que implemente esta interfaz para evitar
         *     algún error de implementación.
        */


        // Método para crear un nuevo producto
        Product CrearProduct(Product product);

        // Método para obtener todos los producto
        List<Product> ObtenerTodosLosProdcuts();

        // Método para obtener todos los producto publicos
        List<Product> ObtenerTodosLosProdcutsPublic();

        // Método para obtener un producto por su ID
        Product ObtenerProductPorId(int productId);

        // Método para obtener un producto por su ID publico
        Product ObtenerProductPorIdPublic(int productId);

        // Método para actualizar información de un producto
        Product ActualizarProduct(int productId, Product productActualizado);

        // Método para eliminar un producto
        bool EliminarProduct(int productId);
    }
}
