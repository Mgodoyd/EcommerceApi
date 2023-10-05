using Microsoft.AspNetCore.Mvc;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Errors;
using api_ecommerce_v1.Helpers;
using Newtonsoft.Json;
using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;

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
        private readonly IDistributedCache _distributedCache;

        public UserController(IUserService userService, Jwthelper jwtHelper, ILoginService loginService, ApplicationDbContext context, IConfiguration configuration, IDistributedCache distributedCache)
        {
            _userService = userService;
            _jwtHelper = jwtHelper;
            _loginService = loginService;
            _context = context;
            _configuration = configuration;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public IActionResult GetAllUser()
        {
            var cacheKey = "AllUsers";
            var cachedUsers = _distributedCache.GetString(cacheKey);

            if (cachedUsers != null)
            {
                var users = JsonConvert.DeserializeObject<List<User>>(cachedUsers);
                return Ok(users);
            }
            else
            {
                var users = _userService.ObtenerTodosLosUser();

                if (users == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Usuarios no encontrados."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                var serializedUsers = JsonConvert.SerializeObject(users);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };
                _distributedCache.SetString(cacheKey, serializedUsers, cacheEntryOptions);

                return Ok(users);
            }
        }

        [HttpGet("admin/{id}")]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public IActionResult ObtenerUserAdminPorId(int id)
        {
            var cacheKey = $"UserAdminById_{id}";
            var cachedUser = _distributedCache.GetString(cacheKey);

            if (cachedUser != null)
            {
                var user = JsonConvert.DeserializeObject<User>(cachedUser);
                return Ok(user);
            }
            else
            {
                var user = _userService.ObtenerUserAdminPorId(id);

                if (user == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Usuario no encontrado."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                var serializedUser = JsonConvert.SerializeObject(user);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };
                _distributedCache.SetString(cacheKey, serializedUser, cacheEntryOptions);

                return Ok(user);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetUserById(int id)
        {
            var cacheKey = $"UserById_{id}";
            var cachedUser = _distributedCache.GetString(cacheKey);

            if (cachedUser != null)
            {
                var user = JsonConvert.DeserializeObject<User>(cachedUser);
                return Ok(user);
            }
            else
            {
                var user = _userService.ObtenerUserPorId(id);

                if (user == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Usuario no encontrado."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                var serializedUser = JsonConvert.SerializeObject(user);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3)
                };
                _distributedCache.SetString(cacheKey, serializedUser, cacheEntryOptions);

                return Ok(user);
            }
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

            var cacheKey = "AllUsers";
            _distributedCache.Remove(cacheKey);


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


            var caheKey2= "AllUsers";
            _distributedCache.Remove(caheKey2);

            var cacheKey3 = $"UserAdminById_{id}";
            _distributedCache.Remove(cacheKey3);
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

            var cacheKey = $"UserById_{id}";
            _distributedCache.Remove(cacheKey);

            var cacheKey2 = "AllUsers";
            _distributedCache.Remove(cacheKey2);

            var cacheKey3 = $"UserAdminById_{id}";
            _distributedCache.Remove(cacheKey3);

            // Serializar el objeto JSON y devolverlo con una respuesta HTTP 200 (OK)
            var successJsonResponse = JsonConvert.SerializeObject(successResponse);
            return Ok(successJsonResponse);
        }
    }

  

}

