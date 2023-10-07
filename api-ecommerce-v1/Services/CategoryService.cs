using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace api_ecommerce_v1.Services
{
    public class CategoryService : ICategory
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;

        /*
         *  Inyectamos el Servicio 
         */
        public CategoryService(ApplicationDbContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
        }

        /*
         *  Metodo para crear una categoria
         */
        public Category CrearCategory(Category category)
        {
            _context.Category.Add(category);
            _context.SaveChanges();
            return category;
        }

        /*
         *  Método para Obtener una categoria por su Id
         */
        public Category ObtenerCategoryporId(int categoryId)
        {
            var category = _context.Category.Find(categoryId);
            return category;
        }

        /*
         *  Método para obtener todas las categorias
         */
        public List<Category> ObtenerTodosCategory()
        {
            var category = _context.Category.ToList();
            return category;
        }

        /*
         *  Método para eliminar una categoria
         */
        public bool eliminarCategory(int categoryId)
        {
            var category = _context.Category.Find(categoryId);
            if (category == null)
            {
                return false;
            }
            _context.Category.Remove(category);

            var cacheKey = "AllCategories";
            _distributedCache.Remove(cacheKey);

            var cacheKey2 = "AllCategoriesPublic";
            _distributedCache.Remove(cacheKey2);

            _context.SaveChanges();
            return true;
        }

        /*
         *  Método para obtener todas las categorias publicas no requiere token
         */
        public List<Category> ObtenerTodosCategoryPublic()
        {
            var category = _context.Category.ToList();
            return category;
        }
    }
}


