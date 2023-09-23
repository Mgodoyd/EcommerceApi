using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface IGalery
    {
        List<Galery> ObtenerGaleryPorProductId(int productId);
        Galery CrearGalery(Galery galery);
        Galery ActualizarGalery(int productId, Galery galeryActualizado);
        bool EliminarGalery(int galeryId);
    }
}
