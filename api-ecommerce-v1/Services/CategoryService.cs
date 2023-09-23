using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce_v1.Services
{
    public class CategoryService : ICategory
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
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


