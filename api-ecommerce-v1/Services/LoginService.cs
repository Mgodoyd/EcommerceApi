using System;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_ecommerce_v1.Models;
using Microsoft.IdentityModel.Tokens;
using api_ecommerce_v1.helpers;
using Microsoft.Extensions.Configuration;

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

            // Autenticación fallida
            return null;
        }




        public string GenerateJwtToken(Login user)
        {
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            // Calcula la fecha de expiración (30 minutos desde ahora)
            var expiration = DateTime.UtcNow.AddSeconds(180);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(expiration).ToUnixTimeSeconds().ToString()), // Agrega el tiempo de expiración
                new Claim("id", user.Id.ToString()),
                new Claim("rol", user.rol.ToString()),
            };

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


        public bool ValidateToken(string token)
        {
            try
            {
                var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwt.Key);

                var claimsPrincipal  = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                // Verificar si el usuario tiene el rol de administrador
                var isAdmin = claimsPrincipal.Claims.Any(claim => claim.Type == "rol" && claim.Value == "administrador");
                
                return isAdmin;
            }
            catch (Exception)
            {
                // Si se lanza una excepción, el token no es válido
                return false;
            }
        }
    }

   

}








