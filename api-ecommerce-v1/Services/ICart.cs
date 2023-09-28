using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface ICart
    {
        // Método para crear un nuevo producto
        Cart CrearCart(Cart cart);

        // Método para obtener todos los producto
        List<Cart> ObtenerTodosLosCarts();

        List<Cart> ObtenerCarritoPorUsuario(int userId);

        // Método para obtener un producto por su ID
        Cart ObtenerCartPorId(int cardId);

        // Método para actualizar información de un producto
        Cart ActualizarCart(int cardId, Cart cartActualizado);

        // Método para eliminar un producto
        bool EliminarCart(int cardId);
    }
}
