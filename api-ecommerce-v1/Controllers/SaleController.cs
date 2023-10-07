using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace api_ecommerce_v1.Controllers
{
    [Route("api/Sale")]
    [ApiController]
    [ServiceFilter(typeof(JwtAuthorizationFilter))]
    public class SaleController : ControllerBase
    {
        private readonly ISale _saleService;
        private readonly IDistributedCache _distributedCache;

        /*
         * Inyectamos los servicios
         */
        public SaleController(ISale saleService,IDistributedCache distributedCache)
        {
            _saleService = saleService;
            _distributedCache = distributedCache;
        }

        /*
         * Obtenemos todas las ventas
         */

        [HttpGet]
        public IActionResult GetAllSales()
        {
            var cachedData = _distributedCache.GetString("SalesData");

            if (cachedData != null)
            {
                var sales = JsonConvert.DeserializeObject<IEnumerable<Sales>>(cachedData);
                return Ok(sales);
            }
            else
            {
                var sales = _saleService.ObtenerTodoslasSale();

                var serializedData = JsonConvert.SerializeObject(sales);
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };

                _distributedCache.SetString("SalesData", serializedData, cacheOptions);

                return Ok(sales);
            }
        }

        /*
         *  Obtenemos una venta por su id
        */
        [HttpGet("{id}")]
        public IActionResult GetSalesById(int id)
        {
            var cachedSale = _distributedCache.GetString($"Sale_{id}");

            if (cachedSale != null)
            {
                var sale = JsonConvert.DeserializeObject<Sales>(cachedSale);
                return Ok(sale);
            }
            else
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

                var settings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var serializedSale = JsonConvert.SerializeObject(sale, settings);
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3)
                };

                _distributedCache.SetString($"Sale_{id}", serializedSale, cacheOptions);

                return Ok(sale);
            }
        }

        /*
         * Obtenemos el total de ventas sin importar el estado
        */

        [HttpGet("totalVendido")]
        public IActionResult GetTotalSales()
        {
            var cachedTotalSales = _distributedCache.GetString("TotalSales");

            if (cachedTotalSales != null)
            {
                var totalSales = JsonConvert.DeserializeObject<decimal>(cachedTotalSales);
                var response = new
                {
                    Message = "Total vendido:",
                    TotalVentas = totalSales
                };
                return Ok(response);
            }
            else
            {
                var totalSales = _saleService.ObtenerTotaldeSalesGeneral();
                var serializedTotalSales = JsonConvert.SerializeObject(totalSales);

                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };

                _distributedCache.SetString("TotalSales", serializedTotalSales, cacheOptions);

                var response = new
                {
                    Message = "Total vendido:",
                    TotalVentas = totalSales
                };
                return Ok(response);
            }
        }

        /*
         *  Obtenemos el total de ventas por estado entregado
        */

        [HttpGet("totalVentas")]
        public IActionResult GetTotalSalesCompletado()
        {
            var cachedTotalSalesCompletado = _distributedCache.GetString("TotalSalesCompletado");

            if (cachedTotalSalesCompletado != null)
            {
                var totalSalesCompletado = JsonConvert.DeserializeObject<decimal>(cachedTotalSalesCompletado);
                var response = new
                {
                    Message = "Total ventas completado:",
                    TotalVentasCompletado = totalSalesCompletado
                };
                return Ok(response);
            }
            else
            {
                var totalSalesCompletado = _saleService.ObtenerTotaldeSalesVendido();

                var serializedTotalSalesCompletado = JsonConvert.SerializeObject(totalSalesCompletado);
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };

                _distributedCache.SetString("TotalSalesCompletado", serializedTotalSalesCompletado, cacheOptions);

                var response = new
                {
                    Message = "Total ventas completado:",
                    TotalVentasCompletado = totalSalesCompletado
                };
                return Ok(response);
            }
        }

        /*
         *     Obtenemos el monto total ganado por ventas
         */

        [HttpGet("total")]
        public IActionResult GetTotalSalesTotal()
        {
            var cachedTotalSalesTotal = _distributedCache.GetString("TotalSalesTotal");

            if (cachedTotalSalesTotal != null)
            {
                var totalSalesTotal = JsonConvert.DeserializeObject<decimal>(cachedTotalSalesTotal);
                var response = new
                {
                    Message = "Total ventas:",
                    TotalVentas = totalSalesTotal
                };
                return Ok(response);
            }
            else
            {
                var totalSalesTotal = _saleService.ObtenerTotaldeSalesTotal();

                var serializedTotalSalesTotal = JsonConvert.SerializeObject(totalSalesTotal);
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };

                _distributedCache.SetString("TotalSalesTotal", serializedTotalSalesTotal, cacheOptions);

                var response = new
                {
                    Message = "Total ventas:",
                    TotalVentas = totalSalesTotal
                };
                return Ok(response);
            }
        }

        /*
         * Obtener todas las ventas del usuario por el ID del mismo 
         */

        [HttpGet("user/{id}")]
        public IActionResult GetSaleByUserId(int id)
        {
            var cacheKey = $"SalesByUserId_{id}";
            var cachedSales = _distributedCache.Get(cacheKey);

            if (cachedSales != null)
            {
                var sales = JsonConvert.DeserializeObject<List<Sales>>(Encoding.UTF8.GetString(cachedSales));
                return Ok(sales);
            }
            else
            {
                var sales = _saleService.ObtenerVentasPorUserId(id);

                if (sales == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Venta no encontrada."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                var serializedSales = JsonConvert.SerializeObject(sales);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };
                _distributedCache.Set(cacheKey, Encoding.UTF8.GetBytes(serializedSales), cacheEntryOptions);
                return Ok(sales);
            }
        }

        /*
         *  Método para crear una venta
         */

        [HttpPost]
        public IActionResult CreateSale(Sales sale)
        {
            var saleCreado = _saleService.CrearSale(sale);

            var cacheKey = "SalesData";
            _distributedCache.Remove(cacheKey);

            if (sale.userId != null)
            {
                var cacheKey2 = $"SalesByUserId_{sale.userId}";
                _distributedCache.Remove(cacheKey2);
            }

            return CreatedAtAction(nameof(GetSalesById), new { id = saleCreado.Id }, saleCreado);
        }

        /*
         *    Método para actualizar una venta
         */

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

            var cacheKey = "SalesData";
            _distributedCache.Remove(cacheKey);

            if (sale.userId != null)
            {
                var cacheKey2 = $"SalesByUserId_{sale.userId}";
                _distributedCache.Remove(cacheKey2);

            }

            return Ok(saleActualizado);
        }

        /*
         * Método para eliminar una venta
         */

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
