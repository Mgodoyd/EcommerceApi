using api_ecommerce_v1.Errors;
using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public ConfigController( IConfig configService, ICategory categoryService, IProductBlobConfiguration productBlobConfiguration)
        {
            _configService = configService;
            _categoryService = categoryService;
            _productBlobConfiguration = productBlobConfiguration;
        }

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

            return Ok(configActualizado);
        }

        [HttpGet("{id}")]
        public IActionResult GetConfigById(int id)
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

            return Ok(config);
        }


        [HttpPost]
        public async Task<IActionResult> CrearConfig([FromForm] Config config, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                // Cargar la imagen en Azure Blob Storage y obtener su nombre
                string blobName = await _productBlobConfiguration.UploadFileBlob(imageFile, "ecommerce");
                config.logo = blobName; // Asigna el nombre del blob como URL de la imagen
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
