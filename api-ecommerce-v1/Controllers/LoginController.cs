using Microsoft.AspNetCore.Mvc;
using api_ecommerce_v1.Models;
using Microsoft.AspNetCore.Cors;
using api_ecommerce_v1.Services;

namespace api_ecommerce_v1.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] Login requestData)
        {
            if (requestData == null)
            {
                return BadRequest("Los datos de inicio de sesión no pueden estar vacíos.");
            }

            // Verifica si el cuerpo de la solicitud contiene la contraseña en texto plano
            if (string.IsNullOrWhiteSpace(requestData.password))
            {
                return BadRequest("La contraseña no puede estar vacía.");
            }

            // Utiliza el servicio LoginService para autenticar al usuario y obtener un token JWT
            var token = _loginService.Authenticate(requestData, requestData.password);

            if (token == null)
            {
                // Autenticación fallida
                return Unauthorized("Credenciales incorrectas.");
            }

            return Ok(new
            {
                success = true,
                message = "Autenticación exitosa",
                token = token,
                exp = 1800
            });
        }

        [HttpPut("{email}")]
        public IActionResult UpdatePassword(string email, [FromBody] Login requestData)
        {
            if (requestData == null)
            {
                return BadRequest("Los datos de inicio de sesión no pueden estar vacíos.");
            }

            // Verifica si el cuerpo de la solicitud contiene la contraseña en texto plano
            if (string.IsNullOrWhiteSpace(requestData.password))
            {
                return BadRequest("La contraseña no puede estar vacía.");
            }

            // Utiliza el servicio LoginService para actualizar la contraseña del usuario
            var updatedLogin = _loginService.UpdatePassword(email, requestData);

            if (updatedLogin == null)
            {
                // Actualización fallida
                return NotFound("No se encontró el usuario.");
            }

            return Ok(new
            {
                success = true,
                message = "Contraseña actualizada exitosamente."
            });
        }

    }
}
