using Microsoft.AspNetCore.Mvc;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Cors;

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
                token = token
            });
        }

    }
}
