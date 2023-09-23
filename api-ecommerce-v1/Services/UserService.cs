﻿using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;


namespace api_ecommerce_v1.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para crear un nuevo cliente
        public User CrearUser(User cliente)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(cliente.Login.password);

            // Asignar el hash a la contraseña en lugar del valor en texto plano
            cliente.Login.password = hashedPassword;
            _context.User.Add(cliente);
            _context.SaveChanges();
            return cliente;
        }


        // Método para obtener todos los clientes
        public List<User> ObtenerTodosLosUser()
        {
            var clients = _context.User.Include(c => c.Login).ToList();
            return clients;
        }

        // Método para obtener un cliente por su ID
        public User ObtenerUserPorId(int userId)
        {
            // Filtra los logins por el Id proporcionado
            var login = _context.Login.FirstOrDefault(l => l.Id == userId);

            // Verifica si se encontraron datos
            if (login != null)
            {
                // Encuentra al usuario correspondiente al LoginId
                var userData = _context.User.FirstOrDefault(u => u.LoginId == login.Id);

                return userData;
            }

            // Si no se encontraron datos o no existe un Login con ese Id, puedes retornar null o manejarlo según tus necesidades
            return null;
        }



        // Método para actualizar información de un cliente
        public User ActualizarUser(int clienteId, User clienteActualizado)
        {
            var clienteExistente = _context.User
                .Include(u => u.Login) // Asegúrate de cargar la entidad relacionada
                .FirstOrDefault(c => c.Id == clienteId);

            if (clienteExistente == null)
            {
                return null; // El cliente no existe
            }

            // Actualiza las propiedades de User
            clienteExistente.Name = clienteActualizado.Name;
            clienteExistente.lastname = clienteActualizado.lastname;
            clienteExistente.address = clienteActualizado.address;
            clienteExistente.profile = clienteActualizado.profile;
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

            // Guarda los cambios en la base de datos
            _context.SaveChanges();

            return clienteExistente;
        }

        // Método para eliminar un cliente
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
