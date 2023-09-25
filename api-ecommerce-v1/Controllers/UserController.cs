using Microsoft.AspNetCore.Mvc;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Errors;
using api_ecommerce_v1.Helpers;
using Newtonsoft.Json;
using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Authorization;

namespace api_ecommerce_v1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly Jwthelper _jwtHelper;
        private readonly ILoginService _loginService;
        private readonly ApplicationDbContext _context; 
        public IConfiguration _configuration;

        public UserController(IUserService userService, Jwthelper jwtHelper, ILoginService loginService, ApplicationDbContext context, IConfiguration configuration)
        {
            _userService = userService;
            _jwtHelper = jwtHelper;
            _loginService = loginService;
            _context = context;
            _configuration = configuration;
        }


        [HttpGet]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public IActionResult GetAllUser()
        {
            var clients = _userService.ObtenerTodosLosUser();
            return Ok(clients);
        }

        [HttpGet("admin/{id}")]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public IActionResult  ObtenerUserAdminPorId(int id)
        {
            var users = _userService.ObtenerUserAdminPorId(id);

            if (users == null)
            {
                var errorResponse = new
                {
                    mensaje = "User no encontrado."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(users);
        }

        // GET: api/client/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetUserById(int id)
        {
            var client = _userService.ObtenerUserPorId(id);

            if (client == null)
            {
                var errorResponse = new
                {
                    mensaje = "Usuario no encontrado."
                };

                // Serializar el objeto JSON y devolverlo con una respuesta HTTP 404
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(client);
        }

        // POST: api/client
        [HttpPost]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                var errorResponse = new ErrorResponse
                {
                    Message = "Solicitud no válida",
                    Errors = errors
                };

                return BadRequest(errorResponse);
            }

            var createdClient = _userService.CrearUser(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdClient.Id }, createdClient);
        }

       
       


        // PUT: api/client/{id}
        [HttpPut("{id}")]
        //[ServiceFilter(typeof(JwtAuthorizationFilter))]
        [AllowAnonymous]
        public IActionResult UpdateUser(int id, User user)
        {
            if (user == null || id != user.Id)
            {
                // Crear un objeto JSON personalizado para el mensaje de error
                var errorResponse = new
                {
                    mensaje = "Solicitud no válida."
                };

                // Serializar el objeto JSON y devolverlo con una respuesta HTTP 400 (BadRequest)
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return BadRequest(jsonResponse);
            }

            var updatedClient = _userService.ActualizarUser(id, user);

            if (updatedClient == null)
            {
                // Crear un objeto JSON personalizado para el mensaje de error
                var errorResponse = new
                {
                    mensaje = "Usuario no encontrado."
                };

                // Serializar el objeto JSON y devolverlo con una respuesta HTTP 404 (NotFound)
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(updatedClient);
        }

        // DELETE: api/client/{id}
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public IActionResult DeleteUser(int id)
        {
            var deleted = _userService.EliminarUser(id);

            if (!deleted)
            {
                // Crear un objeto JSON personalizado para el mensaje de error
                var errorResponse = new
                {
                    mensaje = "Usuario no encontrado."
                };

                // Serializar el objeto JSON y devolverlo con una respuesta HTTP 404 (NotFound)
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            // Crear un objeto JSON personalizado para el mensaje de éxito
            var successResponse = new
            {
                mensaje = "Usuario eliminado exitosamente."
            };

            // Serializar el objeto JSON y devolverlo con una respuesta HTTP 200 (OK)
            var successJsonResponse = JsonConvert.SerializeObject(successResponse);
            return Ok(successJsonResponse);
        }
    }

  

}

