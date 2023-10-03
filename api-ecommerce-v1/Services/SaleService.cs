using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce_v1.Services
{
    public class SaleService : ISale
    {
        private readonly ApplicationDbContext _context;

        public SaleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Sales ObtenerSalePorId(int saleId)
        {
            var sale = _context.Sales
                .Include(s => s.user)
                .Include(s => s.address)
                .Include(s => s.nsale)
                .ThenInclude(n => n.products) // Incluir la información de productos relacionados
                .FirstOrDefault(s => s.Id == saleId);

            return sale;
        }


        public List<Sales> ObtenerVentasPorUserId(int userId)
        {
            return _context.Sales.Where(s => s.userId == userId).ToList();
        }


        public List<Sales> ObtenerTodoslasSale()
        {
            return _context.Sales.Include(s => s.user).ToList();
        }

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

        public int ObtenerTotaldeSalesVendido()
        {
            var sales = _context.Sales.Where(s => s.state == "Entregado").ToList();
            var total = sales.Count;
            
            return total;
        }

        public int ObtenerTotaldeSalesTotal()
        {
            var sales = _context.Sales.ToList();
            var total = sales.Count;

            return total;
        }


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
                    // Supongo que nsaleItem.productId es el ID del producto
                    int productId = nsaleItem.productId;

                    // Busca el producto por su ID
                    var product = _context.Product.FirstOrDefault(p => p.Id == productId);

                    if (product != null)
                    {
                        // Resta el stock en función de la cantidad en NSale (amount)
                        product.stock -= nsaleItem.amount;

                        // Asegúrate de validar que el stock no sea negativo aquí
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

            sale.state = "procesando"; // Establece el estado en "procesando"
            _context.Sales.Add(sale); // Agrega la entidad Sale al contexto
            _context.SaveChanges(); // Guarda los cambios en la base de datos

            var deleteCart = _context.Cart.Where(c => c.userId == sale.userId).ToList();
            _context.Cart.RemoveRange(deleteCart);
            _context.SaveChanges();

            return sale;
        }


        public Sales ActualizarSale(int saleId, Sales saleActualizado)
        {
            var sale = _context.Sales.FirstOrDefault(s => s.Id == saleId);
            if (sale != null)
            {
                sale.state = saleActualizado.state;
               // sale.nsale = saleActualizado.nsale;
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
