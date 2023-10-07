using api_ecommerce_v1.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace api_ecommerce_v1.Services
{
    public class CouponService : ICoupon
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;

        /*
         * Inyectamos los Servicios
         */
        public CouponService(ApplicationDbContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
        }

        /*
         * Método para crear un nuevo cupón
         */
        public Coupon CrearCoupon(Coupon coupon)
        {
            _context.Cupon.Add(coupon);
            _context.SaveChanges();
            return coupon;
        }

        /*
         *  Método para obtener todos los cupones
         */
        public List<Coupon> ObtenerTodosCoupon()
        {
            var coupons = _context.Cupon.ToList();
            return coupons;
        }

        /*
         *  Método para obtener un cupón por su id
         */
        public Coupon ObtenerCouponPorId(int couponId)
        {
            return _context.Cupon.FirstOrDefault(p => p.Id == couponId);
        }

        /*
         * Método para validar un cupón
         */
        public Coupon validarCoupon(string code)
        {
             var coupon = _context.Cupon.Where(c => c.limit >= 1).FirstOrDefault(p => p.code == code);
            return coupon;
        }

        /*
         * Método para actualizar un cupón
         */
        public Coupon ActualizarCoupon(int couponId, Coupon couponActualizado)
        {
            var couponExistente = _context.Cupon
                 .FirstOrDefault(p => p.Id == couponId);

            if (couponExistente == null)
            {
                return null;
            }

            couponExistente.code = couponActualizado.code;
            couponExistente.type = couponActualizado.type;
            couponExistente.value = couponActualizado.value;
            couponExistente.limit = couponActualizado.limit;
            _context.SaveChanges();

            return couponExistente;
        }

        /*
         *  Método para eliminar un cupón
         */
        public bool EliminarCoupon(int couponId)
        {
            var couponExistente = _context.Cupon
                 .FirstOrDefault(p => p.Id == couponId);

            if (couponExistente == null)
            {
                return false; 
            }

            _context.Cupon.Remove(couponExistente);

            var cacheKey = $"AllCoupons";
            _distributedCache.Remove(cacheKey);

            _context.SaveChanges();

            return true;
        }
    }
}
