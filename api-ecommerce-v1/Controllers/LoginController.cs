using Microsoft.AspNetCore.Mvc;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;

namespace api_ecommerce_v1.Controllers
{
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
        public IActionResult Login([FromBody] Login requestData )
        {
            if (requestData == null)
            {
                return BadRequest("Los datos de inicio de sesión no pueden estar vacíos.");
            }

            // Utiliza el servicio LoginService para autenticar al usuario y obtener un token JWT
            var token = _loginService.Authenticate(requestData);

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
