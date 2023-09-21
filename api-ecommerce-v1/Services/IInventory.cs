using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface IInventory
    {
        // Método para crear un nuevo producto
        Inventory CrearInventory(Inventory inventory);

        // Método para obtener todos los producto
        List<Inventory> ObtenerTodoInventory();

        // Método para obtener un producto por su ID
        Inventory ObtenerInventoryPorId(int inventoryId);
        List<Inventory> ObtenerInventariosPorProductId(int productId);

        // Método para actualizar información de un producto
        Inventory ActualizarInventory(int inventoryId, Inventory inventoryActualizado);

        // Método para eliminar un producto
        bool EliminarInventory(int inventoryId);
    }
}
