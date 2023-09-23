using api_ecommerce_v1.helpers;
using api_ecommerce_v1.Models;
using api_ecommerce_v1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api_ecommerce_v1.Controllers
{
    [Route("api/Category")]
    [ApiController]
    
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _categoryService;
        public CategoryController(ICategory categoryService)
        {
            _categoryService = categoryService;
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
            var category = _categoryService.ObtenerTodosCategory();
            return Ok(category);
        }

        [HttpGet("public")]
        [AllowAnonymous]
        public IActionResult GetAllCategoryPublic()
        {
            var category = _categoryService.ObtenerTodosCategoryPublic();
            return Ok(category);
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
