using api_ecommerce_v1.Models;

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
               return _context.Sales.FirstOrDefault(s => s.Id == saleId);
        }

        public List<Sales> ObtenerTodoslasSale()
        {
            return _context.Sales.ToList();
        }

        public Sales CrearSale(Sales sale)
        {
            _context.Sales.Add(sale);
            _context.SaveChanges();
            return sale;
        }

        public Sales ActualizarSale(int saleId, Sales saleActualizado)
        {
            var sale = _context.Sales.FirstOrDefault(s => s.Id == saleId);
            if (sale != null)
            {
                sale.state = saleActualizado.state;
                sale.nsale = saleActualizado.nsale;
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
