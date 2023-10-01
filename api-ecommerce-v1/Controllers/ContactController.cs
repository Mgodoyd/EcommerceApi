using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Mvc;

namespace api_ecommerce_v1.Controllers
{
    [Route("api/Contact")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContact _contactService;

        public ContactController(IContact contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public IActionResult GetAllContacts()
        {
            var contacts = _contactService.GetAllContacts();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public IActionResult GetByIdContacts(int id)
        {
            var contact = _contactService.GetByIdContacts(id);

            if (contact == null)
            {
                var errorResponse = new
                {
                    mensaje = "Contact no encontrado."
                };

                return NotFound(errorResponse);
            }

            return Ok(contact);
        }

        [HttpPost]
        public IActionResult CreateContacts(Contact contact)
        {
            var contactCreado = _contactService.CreateContacts(contact);
            return CreatedAtAction(nameof(GetByIdContacts), new { id = contactCreado.Id }, contactCreado);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateContacts(int id, Contact contact)
        {
            var contactActualizado = _contactService.UpdateContacts(id,contact);

            if (contactActualizado == null)
            {
                var errorResponse = new
                {
                    mensaje = "Contact no encontrado."
                };

                return NotFound(errorResponse);
            }

            return Ok(contactActualizado);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteContacts(int id)
        {
            _contactService.DeleteContacts(id);
            return NoContent();
        }
    }
}
