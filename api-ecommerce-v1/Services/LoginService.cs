using System;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_ecommerce_v1.Models;
using Microsoft.IdentityModel.Tokens;
using api_ecommerce_v1.helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce_v1.Services
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        /*
         * Inyectamos los Servicios
         */

        public LoginService(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        /*
         *  Método para autenticar un usuario y validación de la contraseña
         */
        public string Authenticate(Login user, string plainPassword)
        {
            // Buscar el usuario en la base de datos por correo electrónico
            var existingUser = _dbContext.Login.FirstOrDefault(u => u.email == user.email);

            // Verificar si el usuario existe
            if (existingUser != null)
            {
                // Verificar la contraseña encriptada
                if (BCrypt.Net.BCrypt.Verify(plainPassword, existingUser.password))
                {
                    // Autenticación exitosa
                    return GenerateJwtToken(existingUser);
                }
            }
            return null;
        }

        /*
         *  Método para generar el token JWT tipo sh256
         */
        public string GenerateJwtToken(Login login)
        {
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            var expiration = DateTime.UtcNow.AddSeconds(3600);

            var claimsList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(expiration).ToUnixTimeSeconds().ToString()),
                new Claim("id", login.Id.ToString()),
                new Claim("rol", login.rol.ToString()),
        
        };

            var claims = claimsList.ToArray();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
            );

            // Genera el token como una cadena
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /*
         *  Método para validar el token JWT y si el token es válido y contiene los roles de administrador o cliente
         */

        public bool ValidateToken(string token)
        {
            try
            {
                var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwt.Key);

                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                // Verificar si el usuario tiene el rol de administrador
                var isAdmin = claimsPrincipal.Claims.Any(claim => claim.Type == "rol" && claim.Value == "administrador");
                var isClient = claimsPrincipal.Claims.Any(claim => claim.Type == "rol" && claim.Value == "cliente");

                Console.WriteLine("Token validado: " + token);
                Console.WriteLine("Usuario es administrador: " + isAdmin);
                Console.WriteLine("Usuario es cliente: " + isClient);

                return isAdmin || isClient;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al validar token: " + ex.Message);
                return false;
            }
        }

        /*
         *  Método para actualizar la contraseña de un usuario
         */
        public Login UpdatePassword(string email, Login login)
        {
            var existingUser = _dbContext.Login.FirstOrDefault(u => u.email == email);
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(login.password);


            login.password = hashedPassword;

            if (existingUser != null)
            {
                existingUser.password = login.password;
                _dbContext.SaveChanges();
            }

            return existingUser;
        }

    }
}








