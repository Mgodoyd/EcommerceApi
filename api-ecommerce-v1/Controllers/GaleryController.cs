using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
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
        private readonly IDistributedCache _distributedCache;

        /*
         *  Inyectamos los servicios
         */
        public GaleryController(ApplicationDbContext context, IGalery galeryService, IProductBlobConfiguration productBlobConfiguration, IDistributedCache distributedCache)
        {
            _context = context;
            _galeryService = galeryService;
            _productBlobConfiguration = productBlobConfiguration;
            _distributedCache = distributedCache;
        }

        /*
         *  Método para obtener todas las galerías 
         */

        [HttpGet("{galeryId}")]
        public IActionResult GetGaleryById(int galeryId)
        {
            var cacheKey = $"GaleryAll";
            var cachedGalery = _distributedCache.GetString(cacheKey);

            if (cachedGalery != null)
            {
                var galery = JsonConvert.DeserializeObject<List<Galery>>(cachedGalery);
                return Ok(galery);
            }
            else
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

                var serializedGalery = JsonConvert.SerializeObject(galery);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                _distributedCache.SetString(cacheKey, serializedGalery, cacheEntryOptions);

                return Ok(galery);
            }
        }

        /*
         *  Método para crear una galería
         */

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

            var cacheKey = $"GaleryAll";
            _distributedCache.Remove(cacheKey);

            return Ok(galeryCreada);
        }

        /*
         *  Método para actualizar una galería
         */
       /* [HttpPut]
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

            var cacheKey = $"GaleryAll";
            _distributedCache.Remove(cacheKey);

            return Ok(galeryActualizada);
        }*/

        /*
         *  Método para eliminar una galería
         */

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

            var messageResponse = new
            {
                mensaje = "Galeria eliminada correctamente."
            };

            return Ok(messageResponse);
        }
    }
}
