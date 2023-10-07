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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductBlobConfiguration _productBlobConfiguration;
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;

        /*
         * Inyectamos los servicios
         */
        public ProductController(IProductService productService, IProductBlobConfiguration productBlobConfiguration, ApplicationDbContext context, IDistributedCache distributedCache)
        {
            _productService = productService;
            _productBlobConfiguration = productBlobConfiguration;
            _context = context;
            _distributedCache = distributedCache;
        }

        /*
         * Método para obtener todos los productos, requiere token  
         */

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

        /*
         * Método para obtener todos los productos, no requiere token  
        */

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

        /*
         * Método para obtener un producto por id
         */

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

        /*
         *   Método para obtener un producto por id, no requiere token
        */

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

        /*
         *  Método para crear un producto
        */

        [HttpPost]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public async Task<IActionResult> CreateProduct([FromForm] Product product, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                string blobName = await _productBlobConfiguration.UploadFileBlob(imageFile, "ecommerce");
                product.frontpage = blobName;
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

            var cacheKey = "AllProductsPublic";
            _distributedCache.Remove(cacheKey);

            var cacheKey2 = "AllProducts";
            _distributedCache.Remove(cacheKey2);

            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }

        /*
         *  Método para actualizar un producto
        */

        [HttpPut("{id}")]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public async Task<IActionResult> UpdateProduct([FromForm] Product product, int id, IFormFile? imageFile)
        {
            if (imageFile != null)
            {
                string blobName = await _productBlobConfiguration.UploadFileBlob(imageFile, "ecommerce");
                product.frontpage = blobName; 
            }

            var updatedProduct = _productService.ActualizarProduct(id, product);

            if (updatedProduct == null)
            {
                var errorResponse = new
                {
                    mensaje = "Producto no encontrado."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            var cacheKey = "AllProductsPublic";
            _distributedCache.Remove(cacheKey);

            var cacheKey2 = "AllProducts";
            _distributedCache.Remove(cacheKey2);

            var cacheKey3 = $"ProductById_{id}";
            _distributedCache.Remove(cacheKey3);

            return Ok(updatedProduct);
        }

        /*
         *  Método para eliminar un producto
        */

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

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

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

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return StatusCode(500, jsonResponse);
                }
            }

            var successResponse = new
            {
                mensaje = "Producto eliminado exitosamente."
            };

            var successJsonResponse = JsonConvert.SerializeObject(successResponse);


            var cacheKey = "AllProducts";
            _distributedCache.Remove(cacheKey);

            var cacheKey2 = "AllProductsPublic";
            _distributedCache.Remove(cacheKey2);
            return Ok(successJsonResponse);
        }
    }
}
