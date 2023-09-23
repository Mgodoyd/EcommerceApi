using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface ICategory
    {
        List<Category> ObtenerTodosCategory();
        Category CrearCategory(Category category);
        Category ObtenerCategoryporId(int categoryId);
        bool eliminarCategory(int categoryId);
        List<Category> ObtenerTodosCategoryPublic();
    }
}
