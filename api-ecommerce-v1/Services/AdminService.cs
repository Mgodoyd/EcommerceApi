using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce_v1.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para crear un nuevo cliente
        public Admin CrearAdmin(Admin admin)
        {
            // Generar el hash de la contraseña
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(admin.Login.password);

            // Asignar el hash a la contraseña en lugar del valor en texto plano
            admin.Login.password = hashedPassword;

            _context.Administrador.Add(admin);
            _context.SaveChanges();

            return admin;
        }


        // Método para obtener todos los clientes
        public List<Admin> ObtenerTodosLosAdmins()
        {
            var admins = _context.Administrador.Include(a => a.Login).ToList();

            return admins;
        }

        // Método para obtener un cliente por su ID
        public Admin ObtenerAdminPorId(int adminId)
        {
            return _context.Administrador.FirstOrDefault(c => c.Id == adminId);
        }

        // Método para actualizar información de un cliente
        public Admin ActualizarAdmin(int adminId, Admin adminActualizado)
        {
            var adminExistente = _context.Administrador.FirstOrDefault(a => a.Id == adminId);

            if (adminExistente == null)
            {
                return null; // El cliente no existe
            }

            adminExistente.Name = adminActualizado.Name;
            adminExistente.lastname = adminActualizado.lastname;
            adminExistente.phone = adminActualizado.phone;
            // Actualizar otros campos según sea necesario

            _context.SaveChanges();
            return adminExistente;
        }

        // Método para eliminar un cliente
        public bool EliminarAdmin(int adminId)
        {
            var adminExistente = _context.Administrador.FirstOrDefault(a => a.Id == adminId);

            if (adminExistente == null)
            {
                return false; // El cliente no existe
            }

            _context.Administrador.Remove(adminExistente);
            _context.SaveChanges();
            return true;
        }
    }
}

