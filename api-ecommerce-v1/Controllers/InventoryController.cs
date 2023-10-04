using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace api_ecommerce_v1.Controllers
{
    [Route("api/Inventory")]
    [ApiController]
    // [Produces("application/json")]
    [ServiceFilter(typeof(JwtAuthorizationFilter))]
    public class InventoryController : ControllerBase
    {
        private readonly IInventory _inventoryService;
        private readonly IDistributedCache _distributedCache;
        public InventoryController(IInventory inventory, IDistributedCache distributedCache)
        {
            _inventoryService = inventory;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public IActionResult GetAllInventory()
        {
            var cacheKey = "AllInventory";
            var cachedInventory = _distributedCache.GetString(cacheKey);

            if (cachedInventory != null)
            {
                var inventory = JsonConvert.DeserializeObject<List<Inventory>>(cachedInventory);
                return Ok(inventory);
            }
            else
            {
                var inventory = _inventoryService.ObtenerTodoInventory();

                if (inventory == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Inventario no encontrado."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                var serializedInventory = JsonConvert.SerializeObject(inventory);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                _distributedCache.SetString(cacheKey, serializedInventory, cacheEntryOptions);

                return Ok(inventory);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetInventoryById(int id)
        {
            var cacheKey = $"InventoryById_{id}";
            var cachedInventory = _distributedCache.GetString(cacheKey);

            if (cachedInventory != null)
            {
                var inventory = JsonConvert.DeserializeObject<Inventory>(cachedInventory);
                return Ok(inventory);
            }
            else
            {
                var inventory = _inventoryService.ObtenerInventoryPorId(id);

                if (inventory == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Inventario no encontrado."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                var serializedInventory = JsonConvert.SerializeObject(inventory);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                _distributedCache.SetString(cacheKey, serializedInventory, cacheEntryOptions);

                return Ok(inventory);
            }
        }

        [HttpGet("product/{id}")]
        public IActionResult ObtenerInventariosPorProductId(int productId)
        {
            var cacheKey = $"InventariosByProductId_{productId}";
            var cachedInventory = _distributedCache.GetString(cacheKey);

            if (cachedInventory != null)
            {
                var inventory = JsonConvert.DeserializeObject<List<Inventory>>(cachedInventory);
                return Ok(inventory);
            }
            else
            {
                var inventory = _inventoryService.ObtenerInventariosPorProductId(productId);

                if (inventory == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Inventario no encontrado."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                var serializedInventory = JsonConvert.SerializeObject(inventory);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                _distributedCache.SetString(cacheKey, serializedInventory, cacheEntryOptions);

                return Ok(inventory);
            }
        }

        [HttpPost]
        public IActionResult CreateInventory([FromForm] Inventory inventory)
        {
            var _inventory = _inventoryService.CrearInventory(inventory);
            return Ok(_inventory);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateInventory(int id, Inventory inventory)
        {
            var inventoryActualizado = _inventoryService.ActualizarInventory(id, inventory);

            if (inventoryActualizado == null)
            {
                var errorResponse = new
                {
                    mensaje = "Producto no encontrado."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(inventoryActualizado);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteInventory(int id)
        {
            var inventoryEliminado = _inventoryService.EliminarInventory(id);

            if (!inventoryEliminado)
            {
                var errorResponse = new
                {
                    mensaje = "Producto no encontrado."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok();
        }
    }
}
