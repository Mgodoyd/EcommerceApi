using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api_ecommerce_v1.Controllers
{
    [Route("api/Galery")]
    [ApiController]
    [ServiceFilter(typeof(JwtAuthorizationFilter))]
    public class GaleryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IGalery _galeryService;
        private readonly IProductBlobConfiguration _productBlobConfiguration;

        public GaleryController(ApplicationDbContext context, IGalery galeryService, IProductBlobConfiguration productBlobConfiguration)
        {
            _context = context;
            _galeryService = galeryService;
            _productBlobConfiguration = productBlobConfiguration;
        }

        [HttpGet("{galeryId}")]
        public IActionResult GetGaleryById(int galeryId)
        {
            var galery = _galeryService.ObtenerGaleryPorProductId(galeryId);

            if (galery == null)
            {
                var errorResponse = new
                {
                    mensaje = "Galeria no encontrada."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(galery);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGalery([FromForm] Galery galery, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                // Cargar la imagen en Azure Blob Storage y obtener su nombre
                string blobName = await _productBlobConfiguration.UploadFileBlob(imageFile, "ecommerce");

                // Asignar el nombre de la imagen al producto
                galery.galery = blobName;

            }

            var galeryCreada = _galeryService.CrearGalery(galery);

            return Ok(galeryCreada);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGalery([FromForm] Galery galery, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                // Cargar la imagen en Azure Blob Storage y obtener su nombre
                string blobName = await _productBlobConfiguration.UploadFileBlob(imageFile, "ecommerce");

                // Asignar el nombre de la imagen al producto
                galery.galery = blobName;
            }

            var galeryActualizada = _galeryService.ActualizarGalery(galery.Id, galery);

            if (galeryActualizada == null)
            {
                var errorResponse = new
                {
                    mensaje = "Galeria no encontrada."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(galeryActualizada);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGalery(int id)
        {
            var galeryEliminada = _galeryService.EliminarGalery(id);

            if (!galeryEliminada)
            {
                var errorResponse = new
                {
                    mensaje = "Galeria no encontrada."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok();
        }
    }
}
