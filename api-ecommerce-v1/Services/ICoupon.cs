using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface ICoupon
    {
        // Método para crear un nuevo producto
        Coupon CrearCoupon(Coupon coupon);

        // Método para obtener todos los producto
        List<Coupon> ObtenerTodosCoupon();

        // Método para obtener un producto por su ID
        Coupon ObtenerCouponPorId(int couponId);

        // Método para actualizar información de un producto
        Coupon ActualizarCoupon(int couponId, Coupon couponActualizado);

        // Método para eliminar un producto
        bool EliminarCoupon(int couponId);
    }
}
