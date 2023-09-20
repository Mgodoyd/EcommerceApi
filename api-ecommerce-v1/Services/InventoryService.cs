using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public class InventoryService : IInventory
    {
        private readonly ApplicationDbContext _context;

        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Inventory CrearInventory(Inventory inventory)
        {
            _context.Inventory.Add(inventory);
            _context.SaveChanges();
            return inventory;
        }

        public Inventory ObtenerInventoryPorId(int inventoryId)
        {
            var inventory = _context.Inventory.Find(inventoryId);
            return inventory;
        }

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

        public List<Inventory> ObtenerTodoInventory()
        {
            var inventory = _context.Inventory.ToList();
            return inventory;
        }
    }
}
