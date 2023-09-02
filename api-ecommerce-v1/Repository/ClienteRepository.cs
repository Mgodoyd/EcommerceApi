using Microsoft.EntityFrameworkCore;
using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Repository
{
    public class ClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
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

        // Método para crear un nuevo cliente
        public void CrearCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
        }

        // Método para actualizar información de un cliente
        public void ActualizarCliente(Cliente cliente)
        {
            _context.Entry(cliente).State = EntityState.Modified;
            _context.SaveChanges();
        }

        // Método para eliminar un cliente
        public void EliminarCliente(int clienteId)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == clienteId);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                _context.SaveChanges();
            }
        }
    }
}
