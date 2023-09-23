using api_ecommerce_v1.Models;
namespace api_ecommerce_v1.Services
{
    public class ConfigService : IConfig
    {
        private readonly ApplicationDbContext _context;
        public ConfigService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Config CrearConfig(Config config)
        {
            _context.Config.Add(config);
            _context.SaveChanges();
            return config;
        }
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

        public Config ObtenerConfyporId(int configId)
        {
            var config = _context.Config.Find(configId);
            return config;
        }
    }
}
