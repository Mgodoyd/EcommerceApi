using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface IAddress
    {
        Address CrearAddress(Address address);

        // Método para obtener todos los producto
        List<Address> ObtenerTodoslasAddress();

        // Método para obtener un producto por su ID
        Address ObtenerAddressPorId(int addressId);

        // Método para actualizar información de un producto
        Address ActualizarAddress(int addressId, Address addressActualizado);

        // Método para eliminar un producto
        bool EliminarAddress(int addressId);
    }
}
