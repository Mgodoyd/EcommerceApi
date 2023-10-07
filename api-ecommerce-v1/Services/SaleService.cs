using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce_v1.Services
{
    public class SaleService : ISale
    {
        private readonly ApplicationDbContext _context;

        /*
         * Inyectamos el servicio de la base de datos 
        */
        public SaleService(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         *  Este método obtiene una venta por su ID, incluyendo la 
         *  información de usuario, dirección y productos relacionados
        */
        public Sales ObtenerSalePorId(int saleId)
        {
            var sale = _context.Sales
                .Include(s => s.user)
                .Include(s => s.address)
                .Include(s => s.nsale)
                .ThenInclude(n => n.products)
                .FirstOrDefault(s => s.Id == saleId);

            return sale;
        }

        /*
         *      Este método obtiene la lista de ventas por el ID del usuario.
        */
        public List<Sales> ObtenerVentasPorUserId(int userId)
        {
            return _context.Sales.Where(s => s.userId == userId).Include(s => s.user).Include(s => s.address).Include(s => s.nsale).ToList();
        }

        /*
         *    Este método obtiene la lista de todas las ventas.
        */
        public List<Sales> ObtenerTodoslasSale()
        {
            return _context.Sales.Include(s => s.user).Include(s => s.address).Include(s => s.nsale).ToList();
        }

        /*
         *    Este método obtiene el total de ventas y lo devuelve el valor como un entero.
        */
        public int ObtenerTotaldeSalesGeneral()
        {
            var sales = _context.Sales.ToList();
            var total = 0;

            foreach (var sale in sales)
            {
                total += sale.subtotal;
            }

            return total;
        }

        /*
         *     Este método obtiene el total de ventas cuando el estado de la venta es igual a Entregado,
         *     de igual forma se realiza el conteo y retorna el valor como entero.
        */
        public int ObtenerTotaldeSalesVendido()
        {
            var sales = _context.Sales.Where(s => s.state == "Entregado").ToList();
            var total = sales.Count;
            return total;
        }

        /*
         *     Este método obtiene el total de ventas sin importar el estado,
         *     de igual forma se realiza el conteo y retorna el valor como entero.
        */
        public int ObtenerTotaldeSalesTotal()
        {
            var sales = _context.Sales.ToList();
            var total = sales.Count;

            return total;
        }

        /*
         *  Este método crea una venta, primero realizamos una validación si el objeto sale es 
         *  diferente a nulo realizamos un foreach para recorrer la lista de productos que se
         *  verifica si existe el producto por su ID, si existe se resta el stock en función de la cantidad,
         *  si no existe el producto se lanza una excepción, si el producto es nulo se lanza una excepción y
         *  también se lanza una excepción si el stock es menor a 0. Si todo sale bien se agrega la venta al contexto
         *  pero el estado de la venta se establece como procesando, se guardan los cambios en la base de datos y se
         *  elimina el carrito del usuario para que el carrito quede vacío.
        */
        public Sales CrearSale(Sales sale)
        {
            if (sale == null)
            {
                throw new ArgumentNullException(nameof(sale), "La entidad Sale no puede ser nula.");
            }

            if (sale.nsale != null)
            {
                foreach (var nsaleItem in sale.nsale)
                {
                    int productId = nsaleItem.productId;
                    var product = _context.Product.FirstOrDefault(p => p.Id == productId);

                    if (product != null)
                    {
                        product.stock -= nsaleItem.amount;

                        if (product.stock < 0)
                        {
                            throw new InvalidOperationException("No hay suficientes productos disponibles en el stock.");
                        }
                    }
                    else
                    {
                        throw new NullReferenceException($"El producto con ID {productId} no se encuentra en la tabla Product.");
                    }
                }
            }

            sale.state = "procesando"; 
            _context.Sales.Add(sale); 
            _context.SaveChanges(); 

            var deleteCart = _context.Cart.Where(c => c.userId == sale.userId).ToList();
            _context.Cart.RemoveRange(deleteCart);
            _context.SaveChanges();

            return sale;
        }

        /*
         *  Este método actualiza una venta, primero realizamos una validación si el objeto sale es 
         *  diferente a nulo, si lo es actualizamos cada campo de la venta y guardamos los cambios en la base de datos.
         */
        public Sales ActualizarSale(int saleId, Sales saleActualizado)
        {
            var sale = _context.Sales.FirstOrDefault(s => s.Id == saleId);
            if (sale != null)
            {
                sale.state = saleActualizado.state;
                sale.userId = saleActualizado.userId;
                sale.addressId = saleActualizado.addressId;
                sale.subtotal = saleActualizado.subtotal;
                sale.envio_title = saleActualizado.envio_title;
                sale.envio_price = saleActualizado.envio_price;
                sale.transaction = saleActualizado.transaction;
                sale.coupon = saleActualizado.coupon;
                sale.note = saleActualizado.note;
                _context.SaveChanges();
            }
            return sale;
        }

        /*
         *     Este método elimina una venta, primero realizamos una validación si el objeto sale es 
         *     diferente a nulo, si lo es eliminamos la venta y guardamos los cambios en la base de datos.
        */
        public bool EliminarSale(int saleId)
        {
            var sale = _context.Sales.FirstOrDefault(s => s.Id == saleId);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
