using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace api_ecommerce_v1.Controllers
{
    [Route("api/Coupon")]
    [ApiController]
    [ServiceFilter(typeof(JwtAuthorizationFilter))]
    public class CouponController : ControllerBase
    {
        private readonly ICoupon _couponService;
        private readonly IDistributedCache _distributedCache;

        public CouponController(ICoupon couponService, IDistributedCache distributedCache)
        {
            _couponService = couponService;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public IActionResult GetAllCoupon()
        {
            var cacheKey = "AllCoupons";
            var cachedCoupons = _distributedCache.GetString(cacheKey);

            if (cachedCoupons != null)
            {
                var coupons = JsonConvert.DeserializeObject<List<Coupon>>(cachedCoupons);
                return Ok(coupons);
            }
            else
            {
                var coupons = _couponService.ObtenerTodosCoupon();

                if (coupons == null || coupons.Count == 0)
                {
                    var errorResponse = new
                    {
                        mensaje = "No se encontraron cupones."
                    };

                    return NotFound(errorResponse);
                }

                var serializedCoupons = JsonConvert.SerializeObject(coupons);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };

                _distributedCache.SetString(cacheKey, serializedCoupons, cacheEntryOptions);

                return Ok(coupons);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCouponById(int id)
        {
            var cacheKey = $"Coupon_{id}";
            var cachedCoupon = _distributedCache.GetString(cacheKey);

            if (cachedCoupon != null)
            {
                var coupon = JsonConvert.DeserializeObject<Coupon>(cachedCoupon);
                return Ok(coupon);
            }
            else
            {
                var coupon = _couponService.ObtenerCouponPorId(id);

                if (coupon == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Cupon no encontrado."
                    };

                    return NotFound(errorResponse);
                }

                var serializedCoupon = JsonConvert.SerializeObject(coupon);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };

                _distributedCache.SetString(cacheKey, serializedCoupon, cacheEntryOptions);

                return Ok(coupon);
            }
        }


        [HttpPost]
        public IActionResult CreateCoupon(Coupon coupon)
        {
            _couponService.CrearCoupon(coupon);

            var cacheKey = $"AllCoupons";

            // Elimina la entrada de caché existente
            _distributedCache.Remove(cacheKey);

            return Ok(coupon);
        }

        [HttpGet("validate/{coupon}")]
        public IActionResult ValidateCoupon(string coupon)
        {
            var cacheKey = $"ValidatedCoupon_{coupon}";
            var cachedValidatedCoupon = _distributedCache.GetString(cacheKey);

            if (cachedValidatedCoupon != null)
            {
                var validatedCoupon = JsonConvert.DeserializeObject<Coupon>(cachedValidatedCoupon);
                return Ok(validatedCoupon);
            }
            else
            {
                var validatedCoupon = _couponService.validarCoupon(coupon);

                if (validatedCoupon == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Cupon no encontrado."
                    };

                    return NotFound(errorResponse);
                }

                var serializedValidatedCoupon = JsonConvert.SerializeObject(validatedCoupon);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };

                _distributedCache.SetString(cacheKey, serializedValidatedCoupon, cacheEntryOptions);

                return Ok(validatedCoupon);
            }
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

            var cacheKey = $"AllCoupons";
            _distributedCache.Remove(cacheKey);

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
