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

        public SaleController(ISale saleService,IDistributedCache distributedCache)
        {
            _saleService = saleService;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public IActionResult GetAllSales()
        {
            // Intenta obtener los datos de la caché
            var cachedData = _distributedCache.GetString("SalesData");

            if (cachedData != null)
            {
                // Si los datos están en la caché, devuélvelos
                var sales = JsonConvert.DeserializeObject<IEnumerable<Sales>>(cachedData);
                return Ok(sales);
            }
            else
            {
                // Si los datos no están en la caché, obtén los datos de tu servicio
                var sales = _saleService.ObtenerTodoslasSale();

                // Convierte los datos a JSON
                var serializedData = JsonConvert.SerializeObject(sales);

                // Almacena los datos en la caché con una expiración de 5 minutos (por ejemplo)
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                _distributedCache.SetString("SalesData", serializedData, cacheOptions);

                return Ok(sales);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetSalesById(int id)
        {
            // Intenta obtener la venta desde la caché de Redis
            var cachedSale = _distributedCache.GetString($"Sale_{id}");

            if (cachedSale != null)
            {
                // Si la venta está en la caché, devuélvela
                var sale = JsonConvert.DeserializeObject<Sales>(cachedSale);
                return Ok(sale);
            }
            else
            {
                // Si la venta no está en la caché, obtén los datos de tu servicio
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

                // Convierte la venta a JSON
                var settings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var serializedSale = JsonConvert.SerializeObject(sale, settings);


                // Almacena la venta en la caché de Redis con una expiración (por ejemplo, 30 minutos)
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };

                _distributedCache.SetString($"Sale_{id}", serializedSale, cacheOptions);

                return Ok(sale);
            }
        }


        [HttpGet("totalVendido")]
        public IActionResult GetTotalSales()
        {
            // Intenta obtener el total de ventas desde la caché
            var cachedTotalSales = _distributedCache.GetString("TotalSales");

            if (cachedTotalSales != null)
            {
                // Si el total de ventas está en la caché, devuélvelo
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
                // Si el total de ventas no está en la caché, obtén los datos de tu servicio
                var totalSales = _saleService.ObtenerTotaldeSalesGeneral();

                // Convierte el total de ventas a JSON
                var serializedTotalSales = JsonConvert.SerializeObject(totalSales);

                // Almacena el total de ventas en la caché con una expiración de 5 minutos (por ejemplo)
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
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


        [HttpGet("totalVentas")]
        public IActionResult GetTotalSalesCompletado()
        {
            // Intenta obtener el total de ventas completadas desde la caché de Redis
            var cachedTotalSalesCompletado = _distributedCache.GetString("TotalSalesCompletado");

            if (cachedTotalSalesCompletado != null)
            {
                // Si el total de ventas completadas está en la caché, devuélvelo
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
                // Si el total de ventas completadas no está en la caché, obtén los datos de tu servicio
                var totalSalesCompletado = _saleService.ObtenerTotaldeSalesVendido();

                // Convierte el total de ventas completadas a JSON
                var serializedTotalSalesCompletado = JsonConvert.SerializeObject(totalSalesCompletado);

                // Almacena el total de ventas completadas en la caché de Redis con una expiración (por ejemplo, 30 minutos)
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

        [HttpGet("total")]
        public IActionResult GetTotalSalesTotal()
        {
            // Intenta obtener el total de ventas totales desde la caché de Redis
            var cachedTotalSalesTotal = _distributedCache.GetString("TotalSalesTotal");

            if (cachedTotalSalesTotal != null)
            {
                // Si el total de ventas totales está en la caché, devuélvelo
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
                // Si el total de ventas totales no está en la caché, obtén los datos de tu servicio
                var totalSalesTotal = _saleService.ObtenerTotaldeSalesTotal();

                // Convierte el total de ventas totales a JSON
                var serializedTotalSalesTotal = JsonConvert.SerializeObject(totalSalesTotal);

                // Almacena el total de ventas totales en la caché de Redis con una expiración (por ejemplo, 30 minutos)
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
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                _distributedCache.Set(cacheKey, Encoding.UTF8.GetBytes(serializedSales), cacheEntryOptions);
                return Ok(sales);
            }
        }



        [HttpPost]
        public IActionResult CreateSale(Sales sale)
        {
            var saleCreado = _saleService.CrearSale(sale);
            return CreatedAtAction(nameof(GetSalesById), new { id = saleCreado.Id }, saleCreado);
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
