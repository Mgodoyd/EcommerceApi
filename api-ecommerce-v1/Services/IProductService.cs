using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface IProductService
    {

        // Método para crear un nuevo producto
        Product CrearProduct(Product product);

        // Método para obtener todos los producto
        List<Product> ObtenerTodosLosProdcuts();

        List<Product> ObtenerTodosLosProdcutsPublic();

        // Método para obtener un producto por su ID
        Product ObtenerProductPorId(int productId);

        Product ObtenerProductPorIdPublic(int productId);

        // Método para actualizar información de un producto
        Product ActualizarProduct(int productId, Product productActualizado);

        // Método para eliminar un producto
        bool EliminarProduct(int productId);
    }
}
