using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public class InventoryService : IInventory
    {
        private readonly ApplicationDbContext _context;

        /*
         *  Inyectamos el Servicio
         */
        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         *  Método para crear un nuevo inventario
         */
        public Inventory CrearInventory(Inventory inventory)
        {
            // Busca el último inventario relacionado con el mismo productId
            var lastInventory = _context.Inventory
                .OrderByDescending(i => i.Id) // Ordena por id de forma descendente para obtener el último
                .FirstOrDefault(i => i.productId == inventory.productId);

            if (lastInventory != null)
            {
                // Si existe un inventario anterior para el mismo producto, crea uno nuevo basado en el anterior
                var newInventory = new Inventory
                {
                    productId = inventory.productId,
                    amount = /*lastInventory.amount + */inventory.amount,
                    supplier = inventory.supplier
                };

                _context.Inventory.Add(newInventory);
                _context.SaveChanges();

                // Actualiza el valor del stock del producto
                var product = _context.Product.Find(inventory.productId);
                if (product != null)
                {
                    product.stock += inventory.amount;
                    _context.SaveChanges();
                }

                return newInventory;
            }

            _context.Inventory.Add(inventory);
            _context.SaveChanges();

            // Actualiza el valor del stock del producto
            var productToUpdate = _context.Product.Find(inventory.productId);
            if (productToUpdate != null)
            {
                productToUpdate.stock += inventory.amount;
                _context.SaveChanges();
            }

            return inventory;
        }

        /*
         *  Método para obtener un inventario por su id
         */
        public Inventory ObtenerInventoryPorId(int inventoryId)
        {
            var inventory = _context.Inventory.Find(inventoryId);
            return inventory;
        }

        /*
         * Método para obtener un inventario por el id de su producto
         */
        public List<Inventory> ObtenerInventariosPorProductId(int productId)
        {
            var inventarios = _context.Inventory
                .Where(i => i.productId == productId)
                .ToList();
            return inventarios;
        }

        /*
         *  Método para eliminar un inventario por su id
         */

        public bool EliminarInventory(int inventoryId)
        {
            var inventory = _context.Inventory.Find(inventoryId);
            if (inventory == null)
            {
                return false;
            }
            _context.Inventory.Remove(inventory);
            _context.SaveChanges();
            return true;
        }

        /*
         *  Método para actualizar un inventario por su id
         */
        public Inventory ActualizarInventory(int inventoryId, Inventory inventoryActualizado)
        {
            var inventory = _context.Inventory.Find(inventoryId);
            if (inventory == null)
            {
                return null;
            }
            inventory.amount = inventoryActualizado.amount;
            inventory.supplier = inventoryActualizado.supplier;
            _context.SaveChanges();
            return inventory;
        }

        /*
         *  Método para obtener todos los inventarios
         */
        public List<Inventory> ObtenerTodoInventory()
        {
            var inventory = _context.Inventory.ToList();
            return inventory;
        }
    }
}
