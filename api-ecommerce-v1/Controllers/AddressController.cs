using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api_ecommerce_v1.Controllers
{
    [Route("api/Address")]
    [ApiController]
    [ServiceFilter(typeof(JwtAuthorizationFilter))]
    public class AddressController : ControllerBase
    {
        private readonly IAddress _addressService;

        public AddressController(IAddress addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public IActionResult GetAllAddress()
        {
            var address = _addressService.ObtenerTodoslasAddress();
            return Ok(address);
        }

        [HttpGet("{id}")]
        public IActionResult GetAddressById(int id)
        {
            var address = _addressService.ObtenerAddressPorId(id);

            if (address == null)
            {
                var errorResponse = new
                {
                    mensaje = "Dirección no encontrada."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(address);
        }

        [HttpPost]
        public IActionResult CreateAddress( Address address)
        {
            var newAddress = _addressService.CrearAddress(address);
            return Ok(newAddress);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAddress(int id, Address address)
        {
            var addressActualizado = _addressService.ActualizarAddress(id, address);

            if (addressActualizado == null)
            {
                var errorResponse = new
                {
                    mensaje = "Dirección no encontrada."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(addressActualizado);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAddress(int id)
        {
            var addressEliminado = _addressService.EliminarAddress(id);

            if (!addressEliminado)
            {
                var errorResponse = new
                {
                    mensaje = "Dirección no encontrada."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok();
        }   
    }
}
