using api_ecommerce_v1.Models;
using System.Security.Cryptography;
using System.Text;


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
            // Encriptar la contraseña antes de guardarla
            cliente.Password = EncriptarContraseña(cliente.Password);

            _context.Clientes.Add(cliente);
            _context.SaveChanges();
            return cliente;
        }

        // Método para encriptar una contraseña utilizando SHA-256
        private string EncriptarContraseña(string contraseña)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] contraseñaBytes = Encoding.UTF8.GetBytes(contraseña);
                byte[] hashBytes = sha256.ComputeHash(contraseñaBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        // Método para obtener todos los clientes
        public List<Cliente> ObtenerTodosLosClientes()
        {
            return _context.Clientes.ToList();
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
            clienteExistente.Email = clienteActualizado.Email;
            clienteExistente.lastname = clienteActualizado.lastname;
            clienteExistente.Country = clienteActualizado.Country;
            clienteExistente.Password = clienteActualizado.Password;
            clienteExistente.Profile = clienteActualizado.Profile;
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
