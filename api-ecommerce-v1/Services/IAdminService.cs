using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface IAdminService
    {
        // Método para crear un nuevo cliente
        Admin CrearAdmin(Admin admin);

        // Método para obtener todos los clientes
        List<Admin> ObtenerTodosLosAdmins();

        // Método para obtener un cliente por su ID
        Admin ObtenerAdminPorId(int adminId);

        // Método para actualizar información de un cliente
        Admin ActualizarAdmin(int adminId, Admin adminActualizado);

        // Método para eliminar un cliente
        bool EliminarAdmin(int adminId);
    }
}
