using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace api_ecommerce_v1.Services
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        public LoginService(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }
        public string Authenticate(Login user)
        {
            // Buscar el usuario en la base de datos por correo electrónico
            var existingUser = _dbContext.Login
                .Include(u => u.Admin) // Incluye la relación con Admin
                .Include(u => u.Cliente) // Incluye la relación con Cliente
                .FirstOrDefault(u => u.email == user.email && u.password == user.password);

            if (existingUser != null)
            {
                string rol = "Cliente"; // Asume "Cliente" por defecto

                if (existingUser.Admin != null)
                {
                    // Si el usuario tiene un registro en la tabla Admin, asigna el rol de administrador
                    rol = "Admin";
                }

                // Autenticación exitosa
                return GenerateJwtToken(existingUser, rol);
            }
            else
            {
                // Autenticación fallida
                return null;
            }
        }


        public string GenerateJwtToken(Login user, string rol)
        {
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()), // Cambié DateTime a DateTimeOffset y convertí a segundos Unix
                new Claim("id", user.Id.ToString()), // Asegurarse de convertir el Id a cadena.
                new Claim("rol", rol) // Asegurarse de convertir RolId a cadena.
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims, // Agregué la lista de claims aquí
                expires: DateTime.UtcNow.AddMinutes(30), // Aumentar la duración del token según tus necesidades.
                signingCredentials: signingCredentials
            );

            // Generar el token como una cadena
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
