using api_ecommerce_v1.Models;
namespace api_ecommerce_v1.Services
{
    public class ConfigService : IConfig
    {
        private readonly ApplicationDbContext _context;

        /*
         * Inyectamos el Servicio
         */
        public ConfigService(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         *  Método para crear un nueva configuración
         */
        public Config CrearConfig(Config config)
        {
            _context.Config.Add(config);
            _context.SaveChanges();
            return config;
        }

        /*
         *  Método para actualizar una configuración
         */
        public Config ActualizarConfig(int configId, Config confiActualizado)
        { 
            
            var config = _context.Config.Find(configId);
            if (config == null)
            {
                return null;
            }
            config.title = confiActualizado.title;
            config.logo = confiActualizado.logo;
            config.serie = confiActualizado.serie;
            config.correlative = confiActualizado.correlative;
            _context.Config.Update(config);
            _context.SaveChanges();
            return config;
        }

        /*
         *  Método para obtener la configuración por id
         */
        public Config ObtenerConfyporId(int configId)
        {
            var config = _context.Config.Find(configId);
            return config;
        }
    }
}
