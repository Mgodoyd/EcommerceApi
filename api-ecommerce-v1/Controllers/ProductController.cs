using Microsoft.AspNetCore.Mvc;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.helpers;
using Newtonsoft.Json;
using api_ecommerce_v1.Errors;
using Microsoft.EntityFrameworkCore;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace api_ecommerce_v1.Controllers
{
    [Route("api/products")]
    [ApiController]
  // [ServiceFilter(typeof(JwtAuthorizationFilter))]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductBlobConfiguration _productBlobConfiguration;
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;

        public ProductController(IProductService productService, IProductBlobConfiguration productBlobConfiguration, ApplicationDbContext context, IDistributedCache distributedCache)
        {
            _productService = productService;
            _productBlobConfiguration = productBlobConfiguration;
            _context = context;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var cacheKey = "AllProducts";
            var cachedProducts = _distributedCache.Get(cacheKey);

            if (cachedProducts != null)
            {
                var products = JsonConvert.DeserializeObject<List<Product>>(Encoding.UTF8.GetString(cachedProducts));
                return Ok(products);
            }
            else
            {
                var products = _productService.ObtenerTodosLosProdcuts();
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var serializedProducts = JsonConvert.SerializeObject(products, settings);

                var encodedProducts = Encoding.UTF8.GetBytes(serializedProducts);

                _distributedCache.Set(cacheKey, encodedProducts, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                });

                return Ok(products);
            }
        }

        [HttpGet("public")]
        [AllowAnonymous]
        public IActionResult GetAllProductsPublic()
        {
            var cacheKey = "AllProductsPublic";
            var cachedProducts = _distributedCache.Get(cacheKey);

            if (cachedProducts != null)
            {
                var products = JsonConvert.DeserializeObject<List<Product>>(Encoding.UTF8.GetString(cachedProducts));
                return Ok(products);
            }
            else
            {
                var products = _productService.ObtenerTodosLosProdcutsPublic();

                if (products.Count > 0)
                {
                    var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };

                    var serializedProducts = JsonConvert.SerializeObject(products, settings);
                    var cacheEntryOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                    };
                    _distributedCache.Set(cacheKey, Encoding.UTF8.GetBytes(serializedProducts), cacheEntryOptions);
                    return Ok(products);
                }
                else
                {
                    return NoContent();
                }
            }
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public IActionResult GetProductById(int id)
        {
            var cacheKey = $"ProductById_{id}";
            var cachedProduct = _distributedCache.Get(cacheKey);

            if (cachedProduct != null)
            {
                var product = JsonConvert.DeserializeObject<Product>(Encoding.UTF8.GetString(cachedProduct));
                return Ok(product);
            }
            else
            {
                var product = _productService.ObtenerProductPorId(id);

                if (product == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Producto no encontrado."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var serializedProducts = JsonConvert.SerializeObject(product, settings);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };
                _distributedCache.Set(cacheKey, Encoding.UTF8.GetBytes(serializedProducts), cacheEntryOptions);
                return Ok(product);
            }
        }

        [HttpGet("public/{id}")]
        [AllowAnonymous]
        public IActionResult GetProductByIdPublic(int id)
        {
            var cacheKey = $"ProductByIdPublic_{id}";
            var cachedProduct = _distributedCache.Get(cacheKey);

            if (cachedProduct != null)
            {
                var product = JsonConvert.DeserializeObject<Product>(Encoding.UTF8.GetString(cachedProduct));
                return Ok(product);
            }
            else
            {
                var product = _productService.ObtenerProductPorIdPublic(id);

                if (product == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Producto no encontrado."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var serializedProducts = JsonConvert.SerializeObject(product, settings);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };
                _distributedCache.Set(cacheKey, Encoding.UTF8.GetBytes(serializedProducts), cacheEntryOptions);
                return Ok(product);
            }
        }


        [HttpPost]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public async Task<IActionResult> CreateProduct([FromForm] Product product, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                // Cargar la imagen en Azure Blob Storage y obtener su nombre
                string blobName = await _productBlobConfiguration.UploadFileBlob(imageFile, "ecommerce");
                product.frontpage = blobName; // Asigna el nombre del blob como URL de la imagen
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

            var newProduct = _productService.CrearProduct(product);
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }



        [HttpPut("{id}")]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public async Task<IActionResult> UpdateProduct([FromForm] Product product, int id, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                // Cargar la imagen en Azure Blob Storage y obtener su nombre
                string blobName = await _productBlobConfiguration.UploadFileBlob(imageFile, "ecommerce");
                product.frontpage = blobName; // Asigna el nombre del blob como URL de la imagen
            }

         /*   if (product == null || id != product.Id)
            {
                // Crear un objeto JSON personalizado para el mensaje de error
                var errorResponse = new
                {
                    mensaje = "Solicitud no válida."
                };

                // Serializar el objeto JSON y devolverlo con una respuesta HTTP 400 (BadRequest)
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return BadRequest(jsonResponse);
            }*/

            var updatedProduct = _productService.ActualizarProduct(id, product);

            if (updatedProduct == null)
            {
                var errorResponse = new
                {
                    mensaje = "Producto no encontrado."
                };

                // Serializar el objeto JSON y devolverlo con una respuesta HTTP 404 (NotFound)
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = _productService.ObtenerProductPorId(id);

            if (product == null)
            {
                var errorResponse = new
                {
                    mensaje = "Producto no encontrado."
                };

                // Serializar el objeto JSON y devolverlo con una respuesta HTTP 404 (NotFound)
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            // Obtén el nombre del archivo del producto si está almacenado en el blob
            string blobName = product.frontpage; // Asegúrate de que esta propiedad coincida con la que almacena el nombre del blob

            if (!string.IsNullOrEmpty(blobName))
            {
                // Elimina el archivo del blob storage
                string blobDeleted = _productBlobConfiguration.DeleteBlob(blobName, "ecommerce");
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Elimina todos los registros de Inventory con el mismo productId
                    var inventoriesToDelete = _context.Inventory.Where(i => i.productId == id);
                    _context.Inventory.RemoveRange(inventoriesToDelete);

                    // Luego, elimina el producto
                    _context.Product.Remove(product);

                    // Guarda los cambios en una transacción
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    var errorResponse = new
                    {
                        mensaje = "Ocurrió un error al eliminar el producto y sus inventarios relacionados."
                    };

                    // Serializar el objeto JSON y devolverlo con una respuesta HTTP 500 (InternalServerError) u otro código de error adecuado
                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return StatusCode(500, jsonResponse);
                }
            }

            var successResponse = new
            {
                mensaje = "Producto eliminado exitosamente."
            };

            // Serializar el objeto JSON y devolverlo con una respuesta HTTP 200 (OK)
            var successJsonResponse = JsonConvert.SerializeObject(successResponse);
            return Ok(successJsonResponse);
        }
    }
}
