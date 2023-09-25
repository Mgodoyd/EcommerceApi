using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface IUserService
    {
        // Método para crear un nuevo cliente
        User CrearUser(User user);

        // Método para obtener todos los clientes
        List<User> ObtenerTodosLosUser();

        // Método para obtener un cliente por su ID
        User ObtenerUserPorId(int userId);

        User ObtenerUserAdminPorId(int userId);

        // Método para actualizar información de un cliente
        User ActualizarUser(int userId, User userActualizado);

        // Método para eliminar un cliente
        bool EliminarUser(int userId);

    }
}
