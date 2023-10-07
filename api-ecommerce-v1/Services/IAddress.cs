using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface IAddress
    {
        /*
         *     Métodos que debe tener la clase que implemente esta interfaz.
         *     
         *     Nota: Todos los métodos deben ser implementados en la clase que implemente esta interfaz para evitar
         *     algún error de implementación.
        */

        // Método para crear una dirección
        Address CrearAddress(Address address);

        // Método para obtener todos las direcciones
        List<Address> ObtenerTodoslasAddress();

        // Método para obtener todas las direcciones de un usuario
        List<Address> ObtenerAddressPorUsuario(int userId);

        // Método para obtener una dirección por su id
        Address ObtenerAddressPorId(int addressId);

        // Método para obtener una dirección por  id del usuario
        Address ObtenerAddressPorUser(int userId);

        // Método para actualizar información de una dirección
        Address ActualizarAddress(int addressId, Address addressActualizado);

        // Método para eliminar una dirección
        bool EliminarAddress(int addressId);
    }
}
