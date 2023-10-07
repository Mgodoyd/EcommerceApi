using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface ICategory
    {

        /*
         *     Métodos que debe tener la clase que implemente esta interfaz.
         *     
         *     Nota: Todos los métodos deben ser implementados en la clase que implemente esta interfaz para evitar
         *     algún error de implementación.
        */

        // Método para obtener todas las categorias.
        List<Category> ObtenerTodosCategory();

        // Método para crear una nueva categoría.
        Category CrearCategory(Category category);

        // Método para actualizar una categoría.
        Category ObtenerCategoryporId(int categoryId);

        // Método para eliminar una categoría.
        bool eliminarCategory(int categoryId);

        // Método para obtener todas las categorias publicas.
        List<Category> ObtenerTodosCategoryPublic();
    }
}
