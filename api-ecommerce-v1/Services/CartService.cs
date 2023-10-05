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

        public bool EliminarCart(int cardId)
        {
            // Obtener el userId del carrito que se va a eliminar
            var cart = _context.Cart.Find(cardId);

            if (cart == null)
            {
                return false;
            }

            // Eliminar la entidad de carrito de la base de datos
            _context.Cart.Remove(cart);


            var cacheKey = $"CartByUserId_{cart.userId}";
            _distributedCache.Remove(cacheKey);

            var cacheKey2 = "AllCarts";
            _distributedCache.Remove(cacheKey2);

            var cacheKey3 = $"CartById_{cardId}";
            _distributedCache.Remove(cacheKey3);

            // Guardar los cambios en la base de datos
            _context.SaveChanges();

            return true;
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
