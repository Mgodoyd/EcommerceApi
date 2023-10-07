using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace api_ecommerce_v1.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        /* Realizamos la inyección del Servicio de la base
           de datos ApplicationDbContext y del servicio de
           caché distribuida IDistributedCache
         */

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

         /*
            Este método se encarga de crear un nuevo usuario en la base de datos.
            al momento de crear un usuario, se debe crear un Login asociado a este
            usuario, la contraseña del usuario se encripta con la librería BCrypt.Net
         */
        public User CrearUser(User user)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Login.password);

            // Asignar el hash a la contraseña en lugar del valor en texto plano
            user.Login.password = hashedPassword;
            _context.User.Add(user);
            _context.SaveChanges();
            return user;
        }

        /*
            Este método se encarga de crear un nuevo usuario en la base de datos.
            al momento de crear un usuario, se debe crear un Login asociado a este
            usuario, la contraseña del usuario se encripta con la librería BCrypt.Net,
            este método es de ambito publico, por lo que no requiere token de autenticación
        */

        public User CrearUserPublic(User user)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Login.password);

            user.Login.password = hashedPassword;
            user.Login.rol = "cliente";
            _context.User.Add(user);
            _context.SaveChanges();
            return user;
        }

        /*
             Este método se encarga de obtener todos los usuarios de la base de datos,
             al momento de obtener los usuarios, se incluye la información del Login
             asociado a cada usuario.
        */
        public List<User> ObtenerTodosLosUser()
        {
            var clients = _context.User.Include(c => c.Login).ToList();
            return clients;
        }

        /*
            Este método se encarga de obtener un usuario por su Id, al momento de obtener
            el usuario, se incluye la información del Login asociado a este usuario.
            Además, se necesita el token de autenticación para poder acceder a este método.
        */
        public User ObtenerUserAdminPorId(int userId)
        {
            return  _context.User.Include(u => u.Login).FirstOrDefault(u => u.Id == userId);
        }

        /*
            Es método se encarga de obtener un usuario por su Id, al momento de obtener
            el usuario, se incluye la información del Login asociado a este usuario.
            Primero filtra los logins por el Id proporcionado, luego verifica si se
            encontraron datos,busca al usuario correspondiente al LoginId y lo retorna.
            Además, este método es de ambito publico, por lo que no requiere token de
            autenticación.
         */
        public User ObtenerUserPorId(int userId)
        {
            // Filtra los logins por el Id proporcionado
            var login = _context.Login.FirstOrDefault(l => l.Id == userId);
            if (login != null)
            {
                // Encuentra al usuario correspondiente al LoginId
                var userData = _context.User.FirstOrDefault(u => u.LoginId == login.Id);
                return userData;
            }
            return null;
        }

        /*
         *       Este método se encarga de actualizar un usuario, al momento de actualizar
         *       el usuario, se actualiza la información del Login asociado a este usuario.
         *       Primero se busca al usuario por su Id, si el usuario no existe, se retorna
         *       Además, se necesita el token de autenticación para poder acceder a este método.
        */
        public User ActualizarUser(int clienteId, User clienteActualizado)
        {
            var clienteExistente = _context.User
                .Include(u => u.Login) 
                .FirstOrDefault(c => c.Id == clienteId);

            if (clienteExistente == null)
            {
                return null; // El cliente no existe
            }

            clienteExistente.Name = clienteActualizado.Name;
            clienteExistente.lastname = clienteActualizado.lastname;
            clienteExistente.address = clienteActualizado.address;
            clienteExistente.phone = clienteActualizado.phone;
            clienteExistente.nit = clienteActualizado.nit;

            // Actualiza las propiedades de Login
            clienteExistente.Login.email = clienteActualizado.Login.email;
            clienteExistente.Login.password = clienteActualizado.Login.password;
            clienteExistente.Login.rol = clienteActualizado.Login.rol;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(clienteActualizado.Login.password);
            clienteExistente.Login.password = hashedPassword;

            // Marca la entidad User como modificada
            _context.Entry(clienteExistente).State = EntityState.Modified;
            _context.SaveChanges();

            return clienteExistente;
        }

        /*
         *       Este método se encarga de eliminar un usuario, al momento de eliminar el usuario,
         *       se elimina la información del Login asociado a este usuario.
         *       Primero se busca al usuario por su Id, si el usuario no existe, se retorna
         *       Además, se necesita el token de autenticación para poder acceder a este método.
        */
        public bool EliminarUser(int clienteId)
        {
            var clienteExistente = _context.User.FirstOrDefault(c => c.Id == clienteId);

            if (clienteExistente == null)
            {
                return false; // El cliente no existe
            }

            _context.User.Remove(clienteExistente);
            _context.SaveChanges();
            return true;
        }
    }
}
