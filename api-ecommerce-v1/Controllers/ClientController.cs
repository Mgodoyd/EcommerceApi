using Microsoft.AspNetCore.Mvc;
using api_ecommerce_v1.Services;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Errors;
using api_ecommerce_v1.Helpers;
using Newtonsoft.Json;
using api_ecommerce_v1.helpers;

namespace api_ecommerce_v1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(JwtAuthorizationFilter))]
    public class ClientController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly Jwthelper _jwtHelper;
        private readonly ILoginService _loginService;
        private readonly ApplicationDbContext _context; 
        public IConfiguration _configuration;

        public ClientController(IClienteService clienteService, Jwthelper jwtHelper, ILoginService loginService, ApplicationDbContext context, IConfiguration configuration)
        {
            _clienteService = clienteService;
            _jwtHelper = jwtHelper;
            _loginService = loginService;
            _context = context;
            _configuration = configuration;
        }


        [HttpGet]
        public IActionResult GetAllClients()
        {
            var clients = _clienteService.ObtenerTodosLosClientes();
            return Ok(clients);
        }










        // GET: api/client/{id}
        [HttpGet("{id}")]
        public IActionResult GetClientById(int id)
        {
            var client = _clienteService.ObtenerClientePorId(id);

            if (client == null)
            {
                var errorResponse = new
                {
                    mensaje = "Cliente no encontrado."
                };

                // Serializar el objeto JSON y devolverlo con una respuesta HTTP 404
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(client);
        }

        // POST: api/client
        [HttpPost]
        public IActionResult CreateClient([FromBody] Cliente cliente)
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

            var createdClient = _clienteService.CrearCliente(cliente);
            return CreatedAtAction(nameof(GetClientById), new { id = createdClient.Id }, createdClient);
        }

       
       


        // PUT: api/client/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateClient(int id, [FromBody] Cliente cliente)
        {
            if (cliente == null || id != cliente.Id)
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

            var updatedClient = _clienteService.ActualizarCliente(id, cliente);

            if (updatedClient == null)
            {
                // Crear un objeto JSON personalizado para el mensaje de error
                var errorResponse = new
                {
                    mensaje = "Cliente no encontrado."
                };

                // Serializar el objeto JSON y devolverlo con una respuesta HTTP 404 (NotFound)
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(updatedClient);
        }

        // DELETE: api/client/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            var deleted = _clienteService.EliminarCliente(id);

            if (!deleted)
            {
                // Crear un objeto JSON personalizado para el mensaje de error
                var errorResponse = new
                {
                    mensaje = "Cliente no encontrado."
                };

                // Serializar el objeto JSON y devolverlo con una respuesta HTTP 404 (NotFound)
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            // Crear un objeto JSON personalizado para el mensaje de éxito
            var successResponse = new
            {
                mensaje = "Cliente eliminado exitosamente."
            };

            // Serializar el objeto JSON y devolverlo con una respuesta HTTP 200 (OK)
            var successJsonResponse = JsonConvert.SerializeObject(successResponse);
            return Ok(successJsonResponse);
        }
    }

  

}

