using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Helpers
{
    public class Jwthelper
    {
        private readonly IConfiguration _configuration;

        /*
         *  Inyectamos la configuración de la aplicación para poder acceder a las variables de entorno
         */
        public Jwthelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /*
         *  Genera un token JWT con los datos del usuario, primero se crea la llave de seguridad, luego se crean los claims
         *  la cual tendrá los datos del usuario, luego se crea el token con los datos del usuario, la expiración del token
         *  y las credenciales de seguridad, finalmente se retorna el token generado.
         *  
         *  Las claims son los datos que se pueden almacenar en el token, en este caso se almacena el id y el nombre del usuario.
        */

        public string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddSeconds(3600), 
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
