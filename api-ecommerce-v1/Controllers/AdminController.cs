using api_ecommerce_v1.Errors;
using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api_ecommerce_v1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(JwtAuthorizationFilter))]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // GET: api/client
        [HttpGet]
        public IActionResult GetAllAdmins()
        {
            var admins = _adminService.ObtenerTodosLosAdmins();
            return Ok(admins);
        }

        // GET: api/client/{id}
        [HttpGet("{id}")]
        public IActionResult GetAdminById(int id)
        {
            var admin = _adminService.ObtenerAdminPorId(id);

            if (admin == null)
            {
                var errorResponse = new
                {
                    mensaje = "Admin no encontrado."
                };

                // Serializar el objeto JSON y devolverlo con una respuesta HTTP 404
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(admin);
        }

        // POST: api/client
        [HttpPost]
        public IActionResult CreateAdmin([FromBody] Admin admin)
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

            var createdAdmin = _adminService.CrearAdmin(admin);
            return CreatedAtAction(nameof(GetAdminById), new { id = createdAdmin.Id }, createdAdmin);
        }


        // PUT: api/admin/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateAdmin(int id, [FromBody] Admin admin)
        {
            if (admin == null || id != admin.Id)
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

            var updatedAdmin = _adminService.ActualizarAdmin(id, admin);

            if (updatedAdmin == null)
            {
                // Crear un objeto JSON personalizado para el mensaje de error
                var errorResponse = new
                {
                    mensaje = "Admin no encontrado."
                };

                // Serializar el objeto JSON y devolverlo con una respuesta HTTP 404 (NotFound)
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(updatedAdmin);
        }

        // DELETE: api/admin/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteAdmin(int id)
        {
            var deleted = _adminService.EliminarAdmin(id);

            if (!deleted)
            {
                // Crear un objeto JSON personalizado para el mensaje de error
                var errorResponse = new
                {
                    mensaje = "Admin no encontrado."
                };

                // Serializar el objeto JSON y devolverlo con una respuesta HTTP 404 (NotFound)
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            // Crear un objeto JSON personalizado para el mensaje de éxito
            var successResponse = new
            {
                mensaje = "Admin eliminado exitosamente."
            };

            // Serializar el objeto JSON y devolverlo con una respuesta HTTP 200 (OK)
            var successJsonResponse = JsonConvert.SerializeObject(successResponse);
            return Ok(successJsonResponse);
        }
    }
}

