using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api_ecommerce_v1.Controllers
{
    [Route("api/Sale")]
    [ApiController]
    [ServiceFilter(typeof(JwtAuthorizationFilter))]
    public class SaleController : ControllerBase
    {
        private readonly ISale _saleService;

        public SaleController(ISale saleService)
        {
            _saleService = saleService;
        }

        [HttpGet]
        public IActionResult GetAllSales()
        {
            var sales = _saleService.ObtenerTodoslasSale();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        public IActionResult GetSaleById(int id)
        {
            var sale = _saleService.ObtenerSalePorId(id);

            if (sale == null)
            {
                var errorResponse = new
                {
                    mensaje = "Venta no encontrada."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(sale);
        }

        [HttpPost]
        public IActionResult CreateSale(Sales sale)
        {
            var saleCreado = _saleService.CrearSale(sale);
            return CreatedAtAction(nameof(GetSaleById), new { id = saleCreado.Id }, saleCreado);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSale(int id,  Sales sale)
        {
            var saleActualizado = _saleService.ActualizarSale(id, sale);

            if (saleActualizado == null)
            {
                var errorResponse = new
                {
                    mensaje = "Venta no encontrada."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(saleActualizado);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSale(int id)
        {
            var saleEliminado = _saleService.EliminarSale(id);

            if (!saleEliminado)
            {
                var errorResponse = new
                {
                    mensaje = "Venta no encontrada."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok();
        }


    }
}
