﻿using api_ecommerce_v1.helpers;
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

        /*
         *  Inyectamos los servicios
         */
        public CategoryController(ICategory categoryService, IDistributedCache distributedCache)
        {
            _categoryService = categoryService;
            _distributedCache = distributedCache;
        }

        /*
         *  Método para crear una nueva categoría
         */

        [HttpPost]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public IActionResult CreateCategory(Category category)
        {
            _categoryService.CrearCategory(category);

            var cacheKey = "AllCategories";
            _distributedCache.Remove(cacheKey);

            var cacheKey2 = "AllCategoriesPublic";
            _distributedCache.Remove(cacheKey2);

            return Ok(category);
        }

        /*
         *  Método para obtener todas las categorías
         */

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

        /*
         *  Método para obtener todas las categorías públicas no requiere token
         */

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

        /*
         *  Método para eliminar una categoría
         */

        [HttpDelete("{categoryId}")]
        [ServiceFilter(typeof(JwtAuthorizationFilter))]
        public IActionResult DeleteCategory(int categoryId)
        {
            var category = _categoryService.eliminarCategory(categoryId);
            if (!category)
            {
                var errorResponse = new
                {
                    mensaje = "Category no encontrada."
                };

                var jsonResponse = JsonConvert.SerializeObject(errorResponse);
                return NotFound(jsonResponse);
            }

            var successResponse = new
            {
                mensaje = "Category eliminada exitosamente."
            };

            var successJsonResponse = JsonConvert.SerializeObject(successResponse);
            return Ok(successJsonResponse);
        }
    }
}
