using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api_ecommerce_v1.Controllers
{
    [Route("api/Coupon")]
    [ApiController]
    [ServiceFilter(typeof(JwtAuthorizationFilter))]
    public class CouponController : ControllerBase
    {
        private readonly ICoupon _couponService;

        public CouponController(ICoupon couponService)
        {
            _couponService = couponService;
        }

        [HttpGet]
        public IActionResult GetAllCoupon()
        {
            var coupons = _couponService.ObtenerTodosCoupon();
            return Ok(coupons);
        }

        [HttpGet("{id}")]
        public IActionResult GetCouponById(int id)
        {
            var coupon = _couponService.ObtenerCouponPorId(id);

            if (coupon == null)
            {
                var errorResponse = new
                {
                    mensaje = "Cupon no encontrado."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(coupon);
        }

        [HttpPost]
        public IActionResult CreateCoupon(Coupon coupon)
        {
            _couponService.CrearCoupon(coupon);
            return Ok(coupon);
        }

        [HttpGet("validate/{coupon}")]
        public IActionResult ValidateCoupon(string coupon)
        {
            var couponValidado = _couponService.validarCoupon(coupon);

            if (couponValidado == null)
            {
                var errorResponse = new
                {
                    mensaje = "Cupon no encontrado."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(couponValidado);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCoupon(int id, Coupon coupon)
        {
            var couponActualizado = _couponService.ActualizarCoupon(id, coupon);

            if (couponActualizado == null)
            {
                var errorResponse = new
                {
                    mensaje = "Cupon no encontrado."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(couponActualizado);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCoupon(int id)
        {
            var couponEliminado = _couponService.EliminarCoupon(id);

            if (!couponEliminado)
            {
                var errorResponse = new
                {
                    mensaje = "Cupon no encontrado."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }
            var successResponse = new
            {
                mensaje = "Cupon eliminado exitosamente."
            };

            // Serializar el objeto JSON y devolverlo con una respuesta HTTP 200 (OK)
            var successJsonResponse = JsonConvert.SerializeObject(successResponse);
            return Ok(successJsonResponse);
        }

    }
}
