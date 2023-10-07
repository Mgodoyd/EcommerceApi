using api_ecommerce_v1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace api_ecommerce_v1.Services
{
    public class AddressService : IAddress
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;

        /*
         *  Inyectamos los Servicios
         */
        public AddressService(ApplicationDbContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
        }

        /*
         *  Método para crear un nueva Address
         */
        public Address CrearAddress(Address address)
        {
            _context.Add(address);
            _context.SaveChanges();
            return address;
        }

        /*
         *  Método para eliminar una Address
         */
        public bool EliminarAddress(int addressId)
        {
            var address = _context.Address.Find(addressId);

            if (address == null)
            {
                return false;
            }

            _context.Address.Remove(address);

            var cacheKey = $"AddressByUserId_{address.userId}";
            _distributedCache.Remove(cacheKey);

            _context.SaveChanges();
            return true;
        }

        /*
         *  Método para actualizar una Address
         */
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

        /*
         *  Método para obtener la address por id, incluyendo el usuario
         */
        public Address ObtenerAddressPorId(int addressId)
        {
            var address = _context.Address.Include(a => a.user).FirstOrDefault(a => a.Id == addressId);
            return address;
        }

        /*
         *  Método para obtener la address por id de usuario, incluyendo el usuario cuando la dirección es principal
         */
        public Address ObtenerAddressPorUser(int userId)
        {
            var address = _context.Address.Include(a => a.user).Where(a => a.main == true).FirstOrDefault(a => a.userId == userId);
            return address;
        }

        /*
         *    Método para obtener todas las address de un usuario
         */
        public List<Address> ObtenerAddressPorUsuario(int userId)
        {
            var addressDelUsuario = _context.Address
                .Include(c => c.user)
                .Where(c => c.userId == userId)
                .ToList();

            return addressDelUsuario;
        }

        /*
         *  Método para obtener todas las address, incluyendo el usuario
         */
        public List<Address> ObtenerTodoslasAddress()
        {
            var address = _context.Address.Include(a => a.user).ToList();
            return address;
        }
    }
}
