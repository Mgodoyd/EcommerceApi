using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace api_ecommerce_v1.Services
{
    public class CategoryService : ICategory
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;

        public CategoryService(ApplicationDbContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
        }

        public Category CrearCategory(Category category)
        {
            _context.Category.Add(category);
            _context.SaveChanges();
            return category;
        }

        public Category ObtenerCategoryporId(int categoryId)
        {
            var category = _context.Category.Find(categoryId);
            return category;
        }

        public List<Category> ObtenerTodosCategory()
        {
            var category = _context.Category.ToList();
            return category;
        }

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

        public List<Category> ObtenerTodosCategoryPublic()
        {
            var category = _context.Category.ToList();
            return category;
        }
    }
}


