using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface IClienteService
    {
        // Método para crear un nuevo cliente
        Cliente CrearCliente(Cliente cliente);

        // Método para obtener todos los clientes
        List<Cliente> ObtenerTodosLosClientes();

        // Método para obtener un cliente por su ID
        Cliente ObtenerClientePorId(int clienteId);

        // Método para actualizar información de un cliente
        Cliente ActualizarCliente(int clienteId, Cliente clienteActualizado);

        // Método para eliminar un cliente
        bool EliminarCliente(int clienteId);

    }
}
