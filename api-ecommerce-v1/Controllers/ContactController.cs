using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace api_ecommerce_v1.Controllers
{
    [Route("api/Contact")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContact _contactService;
        private readonly IDistributedCache _distributedCache;

        /*
         *  Inyectamos los Servicios
         */
        public ContactController(IContact contactService, IDistributedCache distributedCache)
        {
            _contactService = contactService;
            _distributedCache = distributedCache;
        }

        /*
         *  Método para obtener todos los contactos
         */

        [HttpGet]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public IActionResult GetAllContacts()
        {
            var cacheKey = "AllContacts";
            var cachedContacts = _distributedCache.GetString(cacheKey);

            if (cachedContacts != null)
            {
                var contacts = JsonConvert.DeserializeObject<List<Contact>>(cachedContacts);
                return Ok(contacts);
            }
            else
            {
                var contacts = _contactService.GetAllContacts();

                if (contacts == null || contacts.Count == 0)
                {
                    var errorResponse = new
                    {
                        mensaje = "No se encontraron contactos."
                    };

                    return NotFound(errorResponse);
                }

                var serializedContacts = JsonConvert.SerializeObject(contacts);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };

                _distributedCache.SetString(cacheKey, serializedContacts, cacheEntryOptions);

                return Ok(contacts);
            }
        }

        /*
         * Método para obtener un contacto por su id
         */

        [HttpGet("{id}")]
        public IActionResult GetByIdContacts(int id)
        {
            var cacheKey = $"Contact_{id}";
            var cachedContact = _distributedCache.GetString(cacheKey);

            if (cachedContact != null)
            {
                var contact = JsonConvert.DeserializeObject<Contact>(cachedContact);
                return Ok(contact);
            }
            else
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

                var serializedContact = JsonConvert.SerializeObject(contact);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };

                _distributedCache.SetString(cacheKey, serializedContact, cacheEntryOptions);

                return Ok(contact);
            }
        }

        /*
         *  Método para crear un contacto
         */

        [HttpPost]
        public IActionResult CreateContacts(Contact contact)
        {
            var contactCreado = _contactService.CreateContacts(contact);

            var cacheKey = $"AllContacts";
            _distributedCache.Remove(cacheKey);

            return CreatedAtAction(nameof(GetByIdContacts), new { id = contactCreado.Id }, contactCreado);
        }

        /*
         *  Método para actualizar un contacto
         */

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

            var cacheKey = $"AllContacts";
            _distributedCache.Remove(cacheKey);

            return Ok(contactActualizado);
        }

        /*
         *  Método para eliminar un contacto
         */

        [HttpDelete("{id}")]
        public IActionResult DeleteContacts(int id)
        {
            _contactService.DeleteContacts(id);
            return NoContent();
        }
    }
}
