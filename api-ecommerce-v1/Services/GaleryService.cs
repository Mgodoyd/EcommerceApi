using api_ecommerce_v1.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace api_ecommerce_v1.Services
{
    public class GaleryService : IGalery
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;

        /*
         *  Inyectamos el Servicio
         */
        public GaleryService(ApplicationDbContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
        }

        /*
         *  Método para crear un nuevo Galery
         */
        public Galery CrearGalery(Galery galery)
        {
            _context.Galery.Add(galery);
            _context.SaveChanges();
            return galery;
        }

        /*
         * Método para obtener la galeria según el id del producto
         */
        public List<Galery> ObtenerGaleryPorProductId(int productId)
        {
            var galery = _context.Galery.Where(p => p.productId == productId).ToList();
            return galery;
        }

        /*
         *  Método para actualizar un Galery
         */
        public Galery ActualizarGalery(int galeryId, Galery galeryActualizado)
        {
            var galeryExistente = _context.Galery
                 .FirstOrDefault(p => p.Id == galeryId);

            if (galeryExistente == null)
            {
                return null;
            }
            galeryExistente.galery = galeryActualizado.galery;
            _context.SaveChanges();

            return galeryExistente;
        }

        /*
         *  Método para eliminar un Galery
         */
        public bool EliminarGalery(int galeryId)
        {
            var galeryExistente = _context.Galery
                 .FirstOrDefault(p => p.Id == galeryId);

            if (galeryExistente == null)
            {
                return false; 
            }

            _context.Galery.Remove(galeryExistente);

           /* var cacheKey = $"GaleryAll{galeryExistente.Id}";
            _distributedCache.Remove(cacheKey);*/
            _context.SaveChanges();

            return true;
        }
    }
}
