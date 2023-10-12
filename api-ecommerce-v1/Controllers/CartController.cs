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

        /*
         *  Inyectamos los servicios
         */
        public CartController(ICart cartService, IDistributedCache distributedCache)
        {
            _cartService = cartService;
            _distributedCache = distributedCache;
        }

        /*
         *  Método para obtener todos los carritos
         */
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
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };

                _distributedCache.SetString(cacheKey, serializedCarts, cacheEntryOptions);

                return Ok(carts);
            }
        }

        /*
         * Método para obtener un carrito por id
         */ 
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

              /*  var serializedCart = JsonConvert.SerializeObject(cart);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };

                _distributedCache.SetString(cacheKey, serializedCart, cacheEntryOptions);*/

                return Ok(cart);
           }
        }

        /*
         *  Método para obtener un carrito por id de usuario
        */
        [HttpGet("user/{userId}")]
        public IActionResult GetCartByUserId(int userId)
        {
           /* try
            {
                var cacheKey = $"CartByUserId_{userId}";

                // Intenta recuperar el carrito desde la caché
                var cachedCart = _distributedCache.GetString(cacheKey);

                if (!string.IsNullOrEmpty(cachedCart))
                {
                    // Si el carrito está en caché, devuélvelo
                    var cart = JsonConvert.DeserializeObject<List<Cart>>(cachedCart);
                    return Ok(cart);
                }*/


                // Si no está en caché, consulta el servicio para obtener el carrito
                var cartFromService = _cartService.ObtenerCarritoPorUsuario(userId);

                if (cartFromService == null)
                {
                    // Si el carrito está vacío, devuelve un mensaje de "no encontrado"
                    var errorResponse = new
                    {
                        mensaje = " No tiene Carrito Asociado"
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                // Serializa y almacena el carrito en caché (usando la clave con el userId)
               /* var serializedCart = JsonConvert.SerializeObject(cartFromService);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    // Puedes configurar opciones de expiración aquí si lo deseas
                };

                _distributedCache.SetString(cacheKey, serializedCart, cacheEntryOptions);*/

                return Ok(cartFromService);
            /*}
            catch (JsonSerializationException ex)
            {
                // Maneja la excepción de deserialización JSON aquí (puedes registrarla para diagnóstico)
                Console.WriteLine($"Error en deserialización JSON: {ex}");
                return StatusCode(500, "Error interno del servidor" + ex);
            }
            catch (Exception ex)
            {
                // Maneja cualquier otra excepción y registra para diagnóstico
                Console.WriteLine($"Error en GetCartByUserId: {ex}");
                return StatusCode(500, "Error interno del servidor" + ex);
            }*/
        }

        /*
         *  Método para crear un carrito
         */
        [HttpPost]
        public IActionResult CreateCart([FromBody] Cart cart)
        {
            var newCart = _cartService.CrearCart(cart);

            if (newCart == null)
            {
                var errorResponse = new
                {
                    mensaje = "No se pudo crear el carrito."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return BadRequest(jsonResponse);
            }

           /* var cacheKey = $"AllCartByUserId{newCart.userId}";
            _distributedCache.Remove(cacheKey);*/

            /*_distributedCache.Remove(cacheKey);*/

            var cacheKey2 = "AllCarts";
            _distributedCache.Remove(cacheKey2);

          /*  var cacheKey3 = $"CartById_{newCart.Id}";
            _distributedCache.Remove(cacheKey3);*/

            return CreatedAtAction(nameof(GetCartById), new { id = newCart.Id }, newCart);
        }

        /*
         *  Método para actualizar un carrito
         */

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

        /*
         *  Método para eliminar un carrito
         */
        [HttpDelete("{id}")]
        public IActionResult DeleteCart(int id)
        {
            var deletedCart = _cartService.EliminarCart(id);

            if (!deletedCart)
            {
                var errorResponse = new
                {
                    mensaje = "Carrito no encontrado."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }
            else
            {
                var successResponse = new
                {
                    mensaje = "Carrito eliminado exitosamente."
                };

                var cacheKey = "AllCarts";
                _distributedCache.Remove(cacheKey);
                var successJsonResponse = JsonConvert.SerializeObject(successResponse);
                return Ok(successJsonResponse);
            }
        }

    }
}
