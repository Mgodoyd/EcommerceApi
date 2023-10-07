using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface IUserService
    {
        /*
         *     Métodos que debe tener la clase que implemente esta interfaz.
         *     
         *     Nota: Todos los métodos deben ser implementados en la clase que implemente esta interfaz para evitar
         *     algún error de implementación.
        */

        // Método para crear un nuevo usuario
        User CrearUser(User user);

        // Método para crear un nuevo usuario publico
        User CrearUserPublic(User user);

        // Método para obtener todos los usuarios
        List<User> ObtenerTodosLosUser();

        // Método para obtener un usuario por su ID
        User ObtenerUserPorId(int userId);

        // Método para obtener un usuario por su ID admin
        User ObtenerUserAdminPorId(int userId);

        // Método para actualizar información de un usuario
        User ActualizarUser(int userId, User userActualizado);

        // Método para eliminar un usuario
        bool EliminarUser(int userId);

    }
}
