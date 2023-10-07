using api_ecommerce_v1.Errors;
using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace api_ecommerce_v1.Controllers
{
    [Route("api/Config")]
    [ApiController]
    [ServiceFilter(typeof(JwtAuthorizationFilter))]
    public class ConfigController : ControllerBase
    {
        private readonly IConfig _configService;
        private readonly ICategory _categoryService;
        private readonly IProductBlobConfiguration _productBlobConfiguration;
        private readonly IDistributedCache _distributedCache;

        /*
         *  Inyecta los servicios necesarios para el controlador
         */
        public ConfigController( IConfig configService, ICategory categoryService, IProductBlobConfiguration productBlobConfiguration, IDistributedCache distributedCache)
        {
            _configService = configService;
            _categoryService = categoryService;
            _productBlobConfiguration = productBlobConfiguration;
            _distributedCache = distributedCache;
        }

        /*
         *  Método para actualizar la configuración de la Tienda 
         */

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConfig(int id, [FromForm] Config config, IFormFile? imageFile)
        {
            if (imageFile != null)
            {
                // Cargar la imagen en Azure Blob Storage y obtener su nombre
                string blobName = await _productBlobConfiguration.UploadFileBlob(imageFile, "ecommerce");
                config.logo = blobName; // Asigna el nombre del blob como URL de la imagen
            }

            var configActualizado = _configService.ActualizarConfig(id, config);
            

            if (configActualizado == null)
            {
                var errorResponse = new
                {
                    mensaje = "Config no encontrado."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            var cacheKey = $"Config_{id}";
            _distributedCache.Remove(cacheKey);

            return Ok(configActualizado);
        }

        /*
         * Método para obtener la configuración de la Tienda por su Id
         */
        [HttpGet("{id}")]
        public IActionResult GetConfigById(int id)
        {
            var cacheKey = $"Config_{id}";
            var cachedConfig = _distributedCache.GetString(cacheKey);

            if (cachedConfig != null)
            {
                var config = JsonConvert.DeserializeObject<Config>(cachedConfig);
                return Ok(config);
            }
            else
            {
                var config = _configService.ObtenerConfyporId(id);

                if (config == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Config no encontrado."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                var serializedConfig = JsonConvert.SerializeObject(config);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };

                _distributedCache.SetString(cacheKey, serializedConfig, cacheEntryOptions);

                return Ok(config);
            }
        }

        /*
         *  Método para Crear una configuración de la Tienda
         */

        [HttpPost]
        public async Task<IActionResult> CrearConfig([FromForm] Config config, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                string blobName = await _productBlobConfiguration.UploadFileBlob(imageFile, "ecommerce");
                config.logo = blobName; 
            }

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

            var newConfig = _configService.CrearConfig(config);
            return CreatedAtAction(nameof(GetConfigById), new { id = newConfig.Id }, newConfig);
        }
    }
}
