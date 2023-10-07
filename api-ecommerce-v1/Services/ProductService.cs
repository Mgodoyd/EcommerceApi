using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace api_ecommerce_v1.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        /*
         * Inyectamos los servicios  
        */
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         *  Método para crear un producto, recibe como parámetro un objeto de tipo Product, primero 
         *  verifica si la categoría existe, si existe asocia el producto con la categoría existente,
         *  si no existe crea una nueva categoría y asocia el producto con ella, luego agrega el producto
         *  a la db, crea el inventario y asigna el id del producto al inventario, luego agrega el
         *  el objeto a la db y guarda los cambios.
         */
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
                _context.Category.Add(product.category); 
            }

            _context.Product.Add(product);
            _context.SaveChanges();

            // Crea el inventario y asigna el productId del producto al inventario.
            var inventory = new Inventory
            {
                productId = product.Id,
                supplier = product.inventory.supplier,
                amount = product.stock
            };

            _context.Inventory.Add(inventory);
            _context.SaveChanges();
            return product;
        }
       
        /*
         *  Método para obtener todos los productos,incluyendo la categoría, el inventario y la galeria.
         */
        public List<Product> ObtenerTodosLosProdcuts()
        {
            var products = _context.Product
                .Include(p => p.inventory).Include(p => p.category).Include(p => p.Galerys).ToList();
            return products;
        }

        /*
         *  Método para obtener todos los productos públicos, incluyendo la categoría,el inventario y la galeria.
         */
        public List<Product> ObtenerTodosLosProdcutsPublic()
        {
            var products= _context.Product.Include(p => p.category).Include(p => p.Galerys).Include(p => p.inventory).ToList();
            return products;
        }

        /*
         *  Método para obtener un producto por su id, incluyendo la categoría, el inventario y la galeria.
        */
        public Product ObtenerProductPorId(int productId)
        {
            return _context.Product
                .Include(p => p.inventory)
                .Include(p => p.category)
                .Include(p => p.Galerys)
                .FirstOrDefault(p => p.Id == productId);
        }

        /*
         *  Método para obtener un producto público por su id, incluyendo la categoría, el inventario y la galeria.
        */
        public Product ObtenerProductPorIdPublic(int productId)
        {
            return _context.Product
                .Include(p => p.inventory)
                .Include(p => p.category)
                .Include(p => p.Galerys) 
                .FirstOrDefault(p => p.Id == productId);
        }

        /*
         *  Método para actualizar un producto, de igual manera que el método para crear un producto,
         *  verifica si la categoría existe, si existe asocia el producto con la categoría existente,
         *  si no existe crea una nueva categoría y asocia el producto con ella, luego actualiza las
         *  columnas de la tabla Product, marca la entidad como modificada y guarda los cambios.
        */
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
                return null; 
            }
            productExistente.title = productActualizado.title;
            productExistente.frontpage = productActualizado.frontpage;
            productExistente.price = productActualizado.price;
            productExistente.description = productActualizado.description;
            productExistente.content = productActualizado.content;
            productExistente.stock = productActualizado.stock;
            productExistente.category = productActualizado.category;
           
            _context.Entry(productExistente).State = EntityState.Modified;
            _context.SaveChanges();
            return productExistente;
        }

        /*
         *  Método para eliminar un producto, primero verifica si el producto existe, si existe
         *  lo elimina de la db y guarda los cambios.
        */
        public bool EliminarProduct(int productId)
        {
            var productExistente = _context.Product.FirstOrDefault(p => p.Id == productId);

            if (productExistente == null)
            {
                return false; 
            }

            _context.Product.Remove(productExistente);
            _context.SaveChanges();
            return true;
        }
    }
}
