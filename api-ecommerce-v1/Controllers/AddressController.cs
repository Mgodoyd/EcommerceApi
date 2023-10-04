using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace api_ecommerce_v1.Controllers
{
    [Route("api/Address")]
    [ApiController]
    [ServiceFilter(typeof(JwtAuthorizationFilter))]
    public class AddressController : ControllerBase
    {
        private readonly IAddress _addressService;
        private readonly IDistributedCache _distributedCache;

        public AddressController(IAddress addressService, IDistributedCache distributedCache)
        {
            _addressService = addressService;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public IActionResult GetAllAddress()
        {
            var cacheKey = "AllAddress";
            var cachedAddress = _distributedCache.Get(cacheKey);

            if (cachedAddress != null)
            {
                var address = JsonConvert.DeserializeObject<List<Address>>(Encoding.UTF8.GetString(cachedAddress));
                return Ok(address);
            }
            else
            {
                var address = _addressService.ObtenerTodoslasAddress();

                if (address == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Dirección no encontrada."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                var serializedAddress = JsonConvert.SerializeObject(address);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                _distributedCache.Set(cacheKey, Encoding.UTF8.GetBytes(serializedAddress), cacheEntryOptions);
                return Ok(address);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetAddressById(int id)
        {
            var cacheKey = $"AddressById_{id}";
            var cachedAddress = _distributedCache.Get(cacheKey);

            if (cachedAddress != null)
            {
                var address = JsonConvert.DeserializeObject<Address>(Encoding.UTF8.GetString(cachedAddress));
                return Ok(address);
            }
            else
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

                var serializedAddress = JsonConvert.SerializeObject(address);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                _distributedCache.Set(cacheKey, Encoding.UTF8.GetBytes(serializedAddress), cacheEntryOptions);
                return Ok(address);
            }
        }

        [HttpGet("address/{id}")]
        public IActionResult GetAddressByUser(int id)
        {
            var cacheKey = $"AddressByUser_{id}";
            var cachedAddress = _distributedCache.Get(cacheKey);

            if (cachedAddress != null)
            {
                var address = JsonConvert.DeserializeObject<Address>(Encoding.UTF8.GetString(cachedAddress));
                return Ok(address);
            }
            else
            {
                var address = _addressService.ObtenerAddressPorUser(id);

                if (address == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Dirección no encontrada."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                var serializedAddress = JsonConvert.SerializeObject(address);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                _distributedCache.Set(cacheKey, Encoding.UTF8.GetBytes(serializedAddress), cacheEntryOptions);
                return Ok(address);
            }
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetAddressByUserId(int userId)
        {
            var cacheKey = $"AddressByUserId_{userId}";
            var cachedAddress = _distributedCache.GetString(cacheKey);

            if (cachedAddress != null)
            {
                // Si hay datos en la caché, devuelve la cadena JSON como JSON
                return Content(cachedAddress, "application/json");
            }
            else
            {
                var address = _addressService.ObtenerAddressPorUsuario(userId);

                if (address == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Dirección no encontrada."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                var serializedAddress = JsonConvert.SerializeObject(address);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };

                // Almacena la dirección como una cadena JSON en la caché de Redis
                _distributedCache.SetString(cacheKey, serializedAddress, cacheEntryOptions);

                // Devuelve la dirección como respuesta JSON
                return Content(serializedAddress, "application/json");
            }
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
