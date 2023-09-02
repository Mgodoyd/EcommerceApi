using Microsoft.AspNetCore.Mvc;
using api_ecommerce_v1.Services; // Asegúrate de importar el espacio de nombres adecuado
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Errors;

namespace api_ecommerce_v1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // GET: api/client
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
                return NotFound(); // Cliente no encontrado
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
                return BadRequest(); // Solicitud no válida
            }

            var updatedClient = _clienteService.ActualizarCliente(id, cliente);

            if (updatedClient == null)
            {
                return NotFound(); // Cliente no encontrado
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
                return NotFound(); // Cliente no encontrado
            }

            return NoContent(); // Éxito en la eliminación
        }
    }
}
