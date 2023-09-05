using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce_v1.Repository
{
    public class AdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para obtener todos los clientes
        public List<Admin> ObtenerTodosLosAdmins()
        {
            return _context.Administrador.ToList();
        }

        // Método para obtener un cliente por su ID
        public Admin ObtenerAdminPorId(int adminId)
        {
            return _context.Administrador.FirstOrDefault(c => c.Id == adminId);
        }

        // Método para crear un nuevo cliente
        public void CrearAdmin(Admin admin)
        {
            _context.Administrador.Add(admin);
            _context.SaveChanges();
        }

        // Método para actualizar información de un cliente
        public void ActualizarAdmin(Admin admin)
        {
            _context.Entry(admin).State = EntityState.Modified;
            _context.SaveChanges();
        }

        // Método para eliminar un cliente
        public void EliminarAdmin(int adminId)
        {
            var admin = _context.Administrador.FirstOrDefault(a => a.Id == adminId);
            if (admin != null)
            {
                _context.Administrador.Remove(admin);
                _context.SaveChanges();
            }
        }
    }
}
