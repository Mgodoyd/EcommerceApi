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

        /*
         * Inyectamos los servicios 
        */
        public UserController(IUserService userService, Jwthelper jwtHelper, ILoginService loginService, ApplicationDbContext context, IConfiguration configuration, IDistributedCache distributedCache)
        {
            _userService = userService;
            _jwtHelper = jwtHelper;
            _loginService = loginService;
            _context = context;
            _configuration = configuration;
            _distributedCache = distributedCache;
        }

        /*
         * Método para obtener todos los usuarios, se utiliza un cache para almacenar los usuarios
         * se realiza una consulta a la base de datos para obtener los usuarios, si no se encuentran
         * en la base de datos se retorna un mensaje de error, si se encuentran se almacenan en el cache
         * se retorna un mensaje de éxito y los usuarios si no se encuentran en el cache primero consulta 
         * en la base de datos y luego los almacena en el cache
        */

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

        /*
         *    Método para obtener el usuario por id, se utiliza un cache para almacenar los usuarios
         *    se realiza una consulta a la base de datos para obtener los usuarios, si no se encuentran
         *    en la base de datos se retorna un mensaje de error, si se encuentran se almacenan en el cache
         *    se retorna un mensaje de éxito y los usuarios si no se encuentran en el cache primero consulta 
         *    en la base de datos y luego los almacena en el cache, este método requiere de un token de autenticación
         */

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

        /*
         * Este método obtiene el usuario por id, se utiliza un cache para almacenar los usuarios
         * si existe el usuario en el cache se retorna el usuario, si no se encuentra en el cache
         * realiza una consulta a la base de datos, si no se encuentra en la base de datos retorna
         * que no se encontró el usuario, si se encuentra en la base de datos se almacena en el cache
         */

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
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };
                _distributedCache.SetString(cacheKey, serializedUser, cacheEntryOptions);

                return Ok(user);
            }
        }

        /*
         * Método para crear un usuario, se valida que el modelo sea válido, si no es válido se retorna
        */
        
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

        /*
         * Método para crear un usuario público, se valida que el modelo sea válido, si no es válido se retorna
        */

        [HttpPost("public")]
        [AllowAnonymous]
        public IActionResult CreateUserPublic([FromBody] User user)
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

            var createdClient = _userService.CrearUserPublic(user);

            var cacheKey = "AllUsers";
            _distributedCache.Remove(cacheKey);

            return CreatedAtAction(nameof(GetUserById), new { id = createdClient.Id }, createdClient);
        }

        /*
         *  Método para actualizar un usuario, se valida que el modelo sea válido, si no es válido se retorna
         *  y se elimina el cache del usuario para resetear el cache
         */
       
        [HttpPut("{id}")]
        [AllowAnonymous]
        public IActionResult UpdateUser(int id, User user)
        {
            if (user == null || id != user.Id)
            {
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
                var errorResponse = new
                {
                    mensaje = "Usuario no encontrado."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            var idLogin = _context.User.Where(x => x.Id == id).Select(x => x.LoginId).FirstOrDefault();

            var cacheKey = $"UserById_{idLogin}";
            _distributedCache.Remove(cacheKey);

            var caheKey2= "AllUsers";
            _distributedCache.Remove(caheKey2);

            var cacheKey3 = $"UserAdminById_{id}";
            _distributedCache.Remove(cacheKey3);
            return Ok(updatedClient);
        }

        /*
         * Método para eliminar un usuario, se valida que el usuario exista, si no existe se retorna un mensaje de error
         */
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public IActionResult DeleteUser(int id)
        {
            var deleted = _userService.EliminarUser(id);

            if (!deleted)
            {
                var errorResponse = new
                {
                    mensaje = "Usuario no encontrado."
                };
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

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

            var successJsonResponse = JsonConvert.SerializeObject(successResponse);
            return Ok(successJsonResponse);
        }
    }

  

}

