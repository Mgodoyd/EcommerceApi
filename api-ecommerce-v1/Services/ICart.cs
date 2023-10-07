using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface ICart
    {
        /*
         *     Métodos que debe tener la clase que implemente esta interfaz.
         *     
         *     Nota: Todos los métodos deben ser implementados en la clase que implemente esta interfaz para evitar
         *     algún error de implementación.
        */

        // Método para crear un nuevo carrito
        Cart CrearCart(Cart cart);

        // Método para obtener todos los carritos
        List<Cart> ObtenerTodosLosCarts();

        // Método para obtener todos los carritos de un usuario
        List<Cart> ObtenerCarritoPorUsuario(int userId);

        // Método para obtener un carrito por su ID
        Cart ObtenerCartPorId(int cardId);

        // Método para actualizar información de un carrito
        Cart ActualizarCart(int cardId, Cart cartActualizado);

        // Método para eliminar un carrito
        bool EliminarCart(int cardId);
    }
}
