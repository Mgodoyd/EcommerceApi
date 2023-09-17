using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce_v1.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Product CrearProduct(Product product )
        {

            // Ahora puedes agregar el producto al contexto y guardar los cambios en la base de datos.
            _context.Product.Add(product);
            _context.SaveChanges();

            return product;
        }


        public List<Product> ObtenerTodosLosProdcuts()
        {
            var products = _context.Product.ToList(); // Quita el .Include(p => p.title)
            return products;
        }


        public Product ObtenerProductPorId(int productId)
        {
            return _context.Product.FirstOrDefault(p => p.Id == productId);
        }



        public Product ActualizarProduct(int productId, Product productActualizado)
        {
            var productExistente = _context.Product
                .FirstOrDefault(p => p.Id == productId);

            if (productExistente == null)
            {
                return null; // El cliente no existe
            }

            // Actualiza las propiedades de User
            productExistente.title = productActualizado.title;
            productExistente.slug = productActualizado.slug;
            productExistente.galery = productActualizado.galery;
            productExistente.frontpage = productActualizado.frontpage;
            productExistente.price = productActualizado.price;
            productExistente.description = productActualizado.description;
            productExistente.content = productActualizado.content;
            productExistente.stock = productActualizado.stock;
            productExistente.sales = productActualizado.sales;
            productExistente.points = productActualizado.points;
            productExistente.category = productActualizado.category;
            productExistente.state = productActualizado.state;

            // Marca la entidad User como modificada
            _context.Entry(productExistente).State = EntityState.Modified;

            // Guarda los cambios en la base de datos
            _context.SaveChanges();

            return productExistente;
        }

        public bool EliminarProduct(int productId)
        {
            var productExistente = _context.User.FirstOrDefault(p => p.Id == productId);

            if (productExistente == null)
            {
                return false; // El producto no existe
            }

            _context.User.Remove(productExistente);
            _context.SaveChanges();
            return true;
        }
    }
}
