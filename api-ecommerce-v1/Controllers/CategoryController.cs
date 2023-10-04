using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace api_ecommerce_v1.Controllers
{
    [Route("api/Category")]
    [ApiController]
    
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _categoryService;
        private readonly IDistributedCache _distributedCache;
        public CategoryController(ICategory categoryService, IDistributedCache distributedCache)
        {
            _categoryService = categoryService;
            _distributedCache = distributedCache;
        }

        [HttpPost]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public IActionResult CreateCategory(Category category)
        {
            _categoryService.CrearCategory(category);
            return Ok(category);
        }

        [HttpGet]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public IActionResult GetAllCategory()
        {
            var cacheKey = "AllCategories";
            var cachedCategories = _distributedCache.GetString(cacheKey);

            if (cachedCategories != null)
            {
                var categories = JsonConvert.DeserializeObject<List<Category>>(cachedCategories);
                return Ok(categories);
            }
            else
            {
                var categories = _categoryService.ObtenerTodosCategory();

                if (categories == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Categorías no encontradas."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                var serializedCategories = JsonConvert.SerializeObject(categories);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                _distributedCache.SetString(cacheKey, serializedCategories, cacheEntryOptions);

                return Ok(categories);
            }
        }

        [HttpGet("public")]
        [AllowAnonymous]
        public IActionResult GetAllCategoryPublic()
        {
            var cacheKey = "AllCategoriesPublic";
            var cachedCategories = _distributedCache.GetString(cacheKey);

            if (cachedCategories != null)
            {
                var categories = JsonConvert.DeserializeObject<List<Category>>(cachedCategories);
                return Ok(categories);
            }
            else
            {
                var categories = _categoryService.ObtenerTodosCategoryPublic();

                if (categories == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "Categorías públicas no encontradas."
                    };

                    var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                    return NotFound(jsonResponse);
                }

                var serializedCategories = JsonConvert.SerializeObject(categories);
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                _distributedCache.SetString(cacheKey, serializedCategories, cacheEntryOptions);

                return Ok(categories);
            }
        }



        [HttpDelete("{categoryId}")]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public IActionResult DeleteCategory(int categoryId)
        {
            var category = _categoryService.eliminarCategory(categoryId);
            if (!category)
            {
                // Crear un objeto JSON personalizado para el mensaje de error
                var errorResponse = new
                {
                    mensaje = "Category no encontrada."
                };

                // Serializar el objeto JSON y devolverlo con una respuesta HTTP 404 (NotFound)
                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            // Crear un objeto JSON personalizado para el mensaje de éxito
            var successResponse = new
            {
                mensaje = "Category eliminada exitosamente."
            };

            // Serializar el objeto JSON y devolverlo con una respuesta HTTP 200 (OK)
            var successJsonResponse = JsonConvert.SerializeObject(successResponse);
            return Ok(successJsonResponse);
        }
    }
}
