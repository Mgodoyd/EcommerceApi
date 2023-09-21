using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public class CouponService : ICoupon
    {
        private readonly ApplicationDbContext _context;

        public CouponService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Coupon CrearCoupon(Coupon coupon)
        {
            _context.Cupon.Add(coupon);
            _context.SaveChanges();
            return coupon;
        }

        public List<Coupon> ObtenerTodosCoupon()
        {
           var coupons = _context.Cupon.ToList();
            return coupons;
        }

        public Coupon ObtenerCouponPorId(int couponId)
        {
            return _context.Cupon.FirstOrDefault(p => p.Id == couponId);
        }

        public Coupon ActualizarCoupon(int couponId, Coupon couponActualizado)
        {
           var couponExistente = _context.Cupon
                .FirstOrDefault(p => p.Id == couponId);

            if (couponExistente == null)
            {
                return null; // El cliente no existe
            }

            // Actualiza los datos del producto existente con los datos del producto actualizado
            couponExistente.code = couponActualizado.code;
            couponExistente.type = couponActualizado.type;
            couponExistente.value = couponActualizado.value;
            couponExistente.limit = couponActualizado.limit;

            // Guarda los cambios en la base de datos
            _context.SaveChanges();

            return couponExistente;
        }

        public bool EliminarCoupon(int couponId)
        {
           var couponExistente = _context.Cupon
                .FirstOrDefault(p => p.Id == couponId);

            if (couponExistente == null)
            {
                return false; // El cliente no existe
            }

            // Elimina el producto del contexto y guarda los cambios en la base de datos
            _context.Cupon.Remove(couponExistente);
            _context.SaveChanges();

            return true;
        }
    }
}
