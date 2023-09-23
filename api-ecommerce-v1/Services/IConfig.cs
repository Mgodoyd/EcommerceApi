using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface IConfig
    {
        Config CrearConfig(Config config);

        Config ActualizarConfig(int configId, Config confiActualizado);

        Config ObtenerConfyporId(int configId);
    }
}
