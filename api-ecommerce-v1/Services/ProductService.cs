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

        public Product CrearProduct(Product product)
        {
            var existingCategory = _context.Category.FirstOrDefault(c => c.titles == product.category.titles);

            if (existingCategory != null)
            {
                // La categoría ya existe, asocia el producto con la categoría existente.
                product.category = existingCategory;
            }
            else
            {
                // La categoría no existe, crea una nueva categoría y asocia el producto con ella.
                _context.Category.Add(product.category); // Asume que product.Category es la categoría asociada al producto.
            }


            // Agrega el producto al contexto y guarda los cambios en la base de datos.
            _context.Product.Add(product);
            _context.SaveChanges();

            // Crea el inventario y asigna el productId del producto al inventario.
            var inventory = new Inventory
            {
                productId = product.Id,
                supplier = product.inventory.supplier,
                amount = product.stock
            };

            // Agrega el inventario al contexto y guarda los cambios en la base de datos.
            _context.Inventory.Add(inventory);
            _context.SaveChanges();

            return product;
        }
       
        public List<Product> ObtenerTodosLosProdcuts()
        {
            var products = _context.Product
                .Include(p => p.inventory).Include(p => p.category).ToList();
            return products;
        }

        public List<Product> ObtenerTodosLosProdcutsPublic()
        {
            var products= _context.Product.Include(p => p.category).Include(p => p.Galerys).Include(p => p.inventory).ToList();
            return products;
        }


        public Product ObtenerProductPorId(int productId)
        {
            return _context.Product.Include(p => p.inventory).Include(p => p.category).Include(p => p.Galerys).FirstOrDefault(p => p.Id == productId);
        }

        public Product ObtenerProductPorIdPublic(int productId)
        {
            return _context.Product
                .Include(p => p.inventory)
                .Include(p => p.category)
                .Include(p => p.Galerys) // Nota la mayúscula en Galerys, que refleja el nombre de la propiedad en la clase Product
                .FirstOrDefault(p => p.Id == productId);
        }



        public Product ActualizarProduct(int productId, Product productActualizado)
        {
            // Verifica si la nueva categoría ya existe en la base de datos.
            var existingCategory = _context.Category.FirstOrDefault(c => c.titles == productActualizado.category.titles);

            if (existingCategory != null)
            {
                // La nueva categoría ya existe, asocia el producto actualizado con la nueva categoría.
                productActualizado.category = existingCategory;
            }
            else
            {
                // La nueva categoría no existe, crea una nueva categoría y asocia el producto actualizado con ella.
                _context.Category.Add(productActualizado.category);
            }

            var productExistente = _context.Product
                .FirstOrDefault(p => p.Id == productId);

            if (productExistente == null)
            {
                return null; // El cliente no existe
            }

            // Actualiza las propiedades de User
            productExistente.title = productActualizado.title;
            productExistente.frontpage = productActualizado.frontpage;
            productExistente.price = productActualizado.price;
            productExistente.description = productActualizado.description;
            productExistente.content = productActualizado.content;
            productExistente.stock = productActualizado.stock;
            productExistente.sales = productActualizado.sales;
            productExistente.points = productActualizado.points;
            productExistente.state = productActualizado.state;
            productExistente.category = productActualizado.category;

            // Marca la entidad User como modificada
            _context.Entry(productExistente).State = EntityState.Modified;

            // Guarda los cambios en la base de datos
            _context.SaveChanges();

            return productExistente;
        }

        public bool EliminarProduct(int productId)
        {
            var productExistente = _context.Product.FirstOrDefault(p => p.Id == productId);

            if (productExistente == null)
            {
                return false; // El producto no existe
            }

            _context.Product.Remove(productExistente);
            _context.SaveChanges();
            return true;
        }
    }
}
