using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface ICoupon
    {
        /*
         *     Métodos que debe tener la clase que implemente esta interfaz.
         *     
         *     Nota: Todos los métodos deben ser implementados en la clase que implemente esta interfaz para evitar
         *     algún error de implementación.
        */

        // Método para crear un nuevo cupon
        Coupon CrearCoupon(Coupon coupon);

        // Método para obtener todos los cupones
        List<Coupon> ObtenerTodosCoupon();

        // Método para obtener un cupon por su ID
        Coupon ObtenerCouponPorId(int couponId);

        // Método para validar un cupon
        Coupon validarCoupon(string code);

        // Método para actualizar información de un cupon
        Coupon ActualizarCoupon(int couponId, Coupon couponActualizado);

        // Método para eliminar un cupon
        bool EliminarCoupon(int couponId);
    }
}
