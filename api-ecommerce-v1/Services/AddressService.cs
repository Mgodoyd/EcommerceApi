using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce_v1.Services
{
    public class AddressService : IAddress
    {
        private readonly ApplicationDbContext _context;

        public AddressService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Address CrearAddress(Address address)
        {
            _context.Add(address);
            _context.SaveChanges();
            return address;
        }

        public bool EliminarAddress(int addressId)
        {
            var address = _context.Address.Find(addressId);

            if (address == null)
            {
                return false;
            }

            _context.Address.Remove(address);
            _context.SaveChanges();
            return true;
        }

        public Address ActualizarAddress(int addressId, Address addressActualizado)
        {
            var address = _context.Address.Find(addressId);

            if (address == null)
            {
                return null;
            }

            address.addressee = addressActualizado.addressee;
            address.dpi = addressActualizado.dpi;
            address.country = addressActualizado.country;
            address.phone = addressActualizado.phone;
            address.zip = addressActualizado.zip;
            address.main = addressActualizado.main;

            _context.SaveChanges();
            return address;
        }

        public Address ObtenerAddressPorId(int addressId)
        {
            var address = _context.Address.Include(a => a.users).FirstOrDefault(a => a.Id == addressId);
            return address;
        }

        public List<Address> ObtenerTodoslasAddress()
        {
            var address = _context.Address.Include(a => a.users).ToList();
            return address;
        }
    }
}
