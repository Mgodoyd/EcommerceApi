using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface IInventory
    {
        /*
         *     Métodos que debe tener la clase que implemente esta interfaz.
         *     
         *     Nota: Todos los métodos deben ser implementados en la clase que implemente esta interfaz para evitar
         *     algún error de implementación.
        */

        // Método para crear un nuevo inventario
        Inventory CrearInventory(Inventory inventory);

        // Método para obtener todos los inventarios
        List<Inventory> ObtenerTodoInventory();

        // Método para obtener un inventario por su ID
        Inventory ObtenerInventoryPorId(int inventoryId);

        // Método para obtener un inventario por id de producto
        List<Inventory> ObtenerInventariosPorProductId(int productId);

        // Método para actualizar información de un inventario
        Inventory ActualizarInventory(int inventoryId, Inventory inventoryActualizado);

        // Método para eliminar un inventario
        bool EliminarInventory(int inventoryId);
    }
}
