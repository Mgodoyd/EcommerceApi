using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public class GaleryService : IGalery
    {
        private readonly ApplicationDbContext _context;

        public GaleryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Galery CrearGalery(Galery galery)
        {
            _context.Galery.Add(galery);
            _context.SaveChanges();
            return galery;
        }

        public List<Galery> ObtenerGaleryPorProductId(int productId)
        {
            var galery = _context.Galery.Where(p => p.productId == productId).ToList();
            return galery;
        }

        public Galery ActualizarGalery(int galeryId, Galery galeryActualizado)
        {
            var galeryExistente = _context.Galery
                 .FirstOrDefault(p => p.Id == galeryId);

            if (galeryExistente == null)
            {
                return null; // El cliente no existe
            }

            // Actualiza los datos del producto existente con los datos del producto actualizado
            galeryExistente.galery = galeryActualizado.galery;

            // Guarda los cambios en la base de datos
            _context.SaveChanges();

            return galeryExistente;
        }

        public bool EliminarGalery(int galeryId)
        {
            var galeryExistente = _context.Galery
                 .FirstOrDefault(p => p.Id == galeryId);

            if (galeryExistente == null)
            {
                return false; // El cliente no existe
            }

            _context.Galery.Remove(galeryExistente);
            _context.SaveChanges();

            return true;
        }
        

    }
}
