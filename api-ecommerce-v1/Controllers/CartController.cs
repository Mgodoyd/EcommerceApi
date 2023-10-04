using api_ecommerce_v1.helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using api_ecommerce_v1.Services;
using Microsoft.Extensions.Caching.Distributed;
using api_ecommerce_v1.Models;
using System.Text;

namespace api_ecommerce_v1.Controllers
{
    [Route("api/Cart")]
    [ApiController]
    [ServiceFilter(typeof(JwtAuthorizationFilter))]
    public class CartController : ControllerBase
    {
        private readonly ICart _cartService;
        private readonly IDistributedCache _distributedCache;

        public CartController(ICart cartService, IDistributedCache distributedCache)
        {
            _cartService = cartService;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public IActionResult GetAllCarts()
        {
            var cacheKey = "AllCarts";
            var cachedCarts = _distributedCache.Get(cacheKey);

            if (cachedCarts != null)
            {
                var carts = JsonConvert.DeserializeObject<List<Cart>>(Encoding.UTF8.GetString(cachedCarts));
                return Ok(carts);
            }
            else
            {
                List<Cart> carts;

                carts = _cartService.ObtenerTodosLosCarts();

                if (carts == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Carritos no encontrados."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                var serializedCarts = JsonConvert.SerializeObject(carts);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                _distributedCache.SetString(cacheKey, serializedCarts, cacheEntryOptions);

                return Ok(carts);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCartById(int id)
        {
            var cacheKey = $"CartById_{id}";
            var cachedCart = _distributedCache.GetString(cacheKey);

            if (cachedCart != null)
            {
                return Ok(JsonConvert.DeserializeObject<Cart>(cachedCart));
            }
            else
            {
                var cart = _cartService.ObtenerCartPorId(id);

                if (cart == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Carrito no encontrado."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                var serializedCart = JsonConvert.SerializeObject(cart);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                _distributedCache.SetString(cacheKey, serializedCart, cacheEntryOptions);

                return Ok(cart);
            }
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetCartByUserId(int userId)
        {
            try
            {
                var cacheKey = $"CartByUserId_{userId}";

                // Intenta recuperar el carrito desde la caché
                var cachedCart = _distributedCache.GetString(cacheKey);

                // Si el carrito está en caché, puedes retornarlo sin necesidad de consultar el servicio
                if (!string.IsNullOrEmpty(cachedCart))
                {
                    var cart = JsonConvert.DeserializeObject<List<Cart>>(cachedCart);
                    return Ok(cart);
                }

                // Si no se encuentra en caché o hubo un problema de deserialización,
                // obtén los datos del servicio y almacénalos en caché
                var cartFromService = _cartService.ObtenerCarritoPorUsuario(userId);

                if (cartFromService == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Carrito no encontrado."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                // Serializa y almacena el carrito en caché (sobrescribiendo si ya existe)
                var serializedCart = JsonConvert.SerializeObject(cartFromService);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                _distributedCache.SetString(cacheKey, serializedCart, cacheEntryOptions);

                return Ok(cartFromService);
            }
            catch (JsonSerializationException ex)
            {
                // Maneja la excepción aquí (puedes registrarla para diagnóstico)
                Console.WriteLine($"Error en deserialización JSON: {ex}");
                return StatusCode(500, "Error interno del servidor" + ex);
            }
            catch (Exception ex)
            {
                // Maneja cualquier otra excepción y registra para diagnóstico
                Console.WriteLine($"Error en GetCartByUserId: {ex}");
                return StatusCode(500, "Error interno del servidor" +ex);
            }
        }








        [HttpPost]
        public IActionResult CreateCart([FromBody] Models.Cart cart)
        {
            var newCart = _cartService.CrearCart(cart);
            return CreatedAtAction(nameof(GetCartById), new { id = newCart.Id }, newCart);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCart(int id, [FromBody] Models.Cart cart)
        {
            var cartUpdated = _cartService.ActualizarCart(id, cart);

            if (cartUpdated == null)
            {
                var errorResponse = new
                {
                    mensaje = "Carrito no encontrado."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(cartUpdated);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCart(int id)
        {
            var cartDeleted = _cartService.EliminarCart(id);

            if (!cartDeleted)
            {
                var errorResponse = new
                {
                    mensaje = "Carrito no encontrado."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok();
        }
       

    }
}
