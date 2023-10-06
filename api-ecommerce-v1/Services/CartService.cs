using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace api_ecommerce_v1.Services
{
    public class CartService : ICart
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;

        public CartService(ApplicationDbContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
        }

        public Cart CrearCart(Cart cart)
        {
            _context.Add(cart);
            var cacheKey = $"CartByUserId_{cart.userId}";
            _distributedCache.Remove(cacheKey);

            var cacheKey2 = "AllCarts";
            _distributedCache.Remove(cacheKey2);

            _context.SaveChanges();
            return cart;
        }

        public bool EliminarCart(int userId)
        {
            try
            {
                // Buscar todos los carritos del usuario en la base de datos
                var userCarts = _context.Cart.Where(c => c.userId == userId).ToList();

                if (userCarts == null || userCarts.Count == 0)
                {
                    return false; // No se encontraron carritos para el usuario
                }

                // Eliminar la entrada de caché para cada carrito del usuario
                foreach (var cart in userCarts)
                {
                    var cacheKey = $"CartByUserId_{userId}";
                    _distributedCache.Remove(cacheKey);
                }

                // Eliminar los carritos de la base de datos
                _context.Cart.RemoveRange(userCarts);

                // Guardar los cambios en la base de datos
                _context.SaveChanges();

                return true; // Todos los carritos del usuario se eliminaron con éxito
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en EliminarCart: {ex}");
                // Manejar la excepción (por ejemplo, registrarla o lanzarla nuevamente)
                // Puedes agregar código aquí para manejar la excepción de acuerdo a tus necesidades
                return false; // Indicar que la eliminación falló debido a una excepción
            }
        }




        public Cart ActualizarCart(int cardId, Cart cartActualizado)
        {
            var cart = _context.Cart.Find(cardId);

            if (cart == null)
            {
                return null;
            }

            cart.amount = cartActualizado.amount;
            cart.productId = cartActualizado.productId;
            cart.userId = cartActualizado.userId;

            _context.SaveChanges();
            return cart;
        }

        public Cart ObtenerCartPorId(int cardId)
        {
            var cart = _context.Cart.Include(c => c.product).Include(c => c.user).FirstOrDefault(c => c.Id == cardId);
            return cart;
        }

        public List<Cart> ObtenerCarritoPorUsuario(int userId)
        {
            // Tu lógica para obtener todos los elementos del carrito asociados al usuario
            var carritoDelUsuario = _context.Cart
                .Include(c => c.products)
                .Include(c => c.users)
                .Where(c => c.userId == userId)
                .ToList();

            return carritoDelUsuario;
        }



        public List<Cart> ObtenerTodosLosCarts()
        {
            var carts = _context.Cart
                .Include(c => c.products)
                .Include(c => c.users)   
                .ToList();

            return carts;
        }

    }
}
