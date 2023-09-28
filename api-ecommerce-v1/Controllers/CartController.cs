using api_ecommerce_v1.helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using api_ecommerce_v1.Services;


namespace api_ecommerce_v1.Controllers
{
    [Route("api/Cart")]
    [ApiController]
    [ServiceFilter(typeof(JwtAuthorizationFilter))]
    public class CartController : ControllerBase
    {
        private readonly ICart _cartService;

        public CartController(ICart cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public IActionResult GetAllCarts()
        {
            var carts = _cartService.ObtenerTodosLosCarts();
            return Ok(carts);
        }

        [HttpGet("{id}")]
        public IActionResult GetCartById(int id)
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

            return Ok(cart);
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetCartByUserId(int userId)
        {
            var cart = _cartService.ObtenerCarritoPorUsuario(userId);

            if (cart == null)
            {
                var errorResponse = new
                {
                    mensaje = "Carrito no encontrado."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(cart);
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
