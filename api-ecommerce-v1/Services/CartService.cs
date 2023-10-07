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
            var cacheKey2 = "AllCarts";
            _distributedCache.Remove(cacheKey2);

            _context.SaveChanges();
            return cart;
        }

        public bool EliminarCart(int Id)
        {
            // Buscar el carrito por su Id en la base de datos
            var cart = _context.Cart.FirstOrDefault(c => c.Id == Id);

            if (cart == null)
            {
                return false; // No se encontró el carrito
            }




            // Buscar todos los carritos del usuario en la base de datos
            // var userCarts = _context.Cart.Where(c => c.userId == userId).ToList();

            _context.Cart.Remove(cart);


            // Eliminar la clave de caché para el carrito eliminado
            var cacheKey3 = $"CartById_{cart.Id}";
            _distributedCache.Remove(cacheKey3);

            


            // Encuentra al usuario en la base de datos
            //  var user = _context.User.SingleOrDefault(u => u.Id == userId);



            /*  if (user != null)
              {*/

            // Eliminar la clave de caché que almacena todos los carritos asociados al usuario
            /*  var cacheKey2 = $"AllCartByUserId";
              _distributedCache.Remove(cacheKey2);*/
            // }

            // Guardar los cambios en la base de datos
            
            _context.SaveChanges();

            return true; // El carrito y todos los carritos del usuario se eliminaron con éxito
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
