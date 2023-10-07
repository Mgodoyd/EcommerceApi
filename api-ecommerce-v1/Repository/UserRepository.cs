using Microsoft.EntityFrameworkCore;
using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Repository
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para obtener todos los usuarios
        public List<User> ObtenerTodosLosUser()
        {
            return _context.User.ToList();
        }

        // Método para obtener un usuario por su ID
        public User ObtenerUserPorId(int userId)
        {
            return _context.User.FirstOrDefault(c => c.Id == userId);
        }

        // Método para crear un nuevo usuario
        public void CrearUser(User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
        }

        // Método para actualizar información de un usuario
        public void ActualizarUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }

        // Método para eliminar un usuario
        public void EliminarUser(int userId)
        {
            var user = _context.User.FirstOrDefault(c => c.Id == userId);
            if (user != null)
            {
                _context.User.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
