using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Mvc;
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
        public InventoryController(IInventory inventory)
        {
            _inventoryService = inventory;
        }

        [HttpGet]
        public IActionResult GetAllInventory()
        {
            var inventory = _inventoryService.ObtenerTodoInventory();
            return Ok(inventory);
        }

        [HttpGet("{id}")]
        public IActionResult GetInventoryById(int id)
        {
            var inventory = _inventoryService.ObtenerInventoryPorId(id);

            if (inventory == null)
            {
                var errorResponse = new
                {
                    mensaje = "Producto no encontrado."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(inventory);
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
