using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;


namespace api_ecommerce_v1.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ApplicationDbContext _context;

        public ClienteService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para crear un nuevo cliente
        public Cliente CrearCliente(Cliente cliente)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(cliente.Login.password);

            // Asignar el hash a la contraseña en lugar del valor en texto plano
            cliente.Login.password = hashedPassword;
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
            return cliente;
        }


        // Método para obtener todos los clientes
        public List<Cliente> ObtenerTodosLosClientes()
        {
            var clients = _context.Clientes.Include(c => c.Login).ToList();
            return clients;
        }

        // Método para obtener un cliente por su ID
        public Cliente ObtenerClientePorId(int clienteId)
        {
            return _context.Clientes.FirstOrDefault(c => c.Id == clienteId);
        }

        // Método para actualizar información de un cliente
        public Cliente ActualizarCliente(int clienteId, Cliente clienteActualizado)
        {
            var clienteExistente = _context.Clientes.FirstOrDefault(c => c.Id == clienteId);

            if (clienteExistente == null)
            {
                return null; // El cliente no existe
            }

            clienteExistente.Name = clienteActualizado.Name;
            clienteExistente.lastname = clienteActualizado.lastname;
            clienteExistente.country = clienteActualizado.country;
            clienteExistente.profile = clienteActualizado.profile;
            clienteExistente.phone = clienteActualizado.phone;
            // Actualizar otros campos según sea necesario

            _context.SaveChanges();
            return clienteExistente;
        }

        // Método para eliminar un cliente
        public bool EliminarCliente(int clienteId)
        {
            var clienteExistente = _context.Clientes.FirstOrDefault(c => c.Id == clienteId);

            if (clienteExistente == null)
            {
                return false; // El cliente no existe
            }

            _context.Clientes.Remove(clienteExistente);
            _context.SaveChanges();
            return true;
        }

       

    }
}
