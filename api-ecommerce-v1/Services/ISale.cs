using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface ISale
    {
        Sales CrearSale(Sales address);

        // Método para obtener todos los producto
        List<Sales> ObtenerTodoslasSale();
        List<Sales> ObtenerVentasPorUserId(int userId);

        int ObtenerTotaldeSalesVendido();

        int ObtenerTotaldeSalesGeneral();

        int ObtenerTotaldeSalesTotal();
        // Método para obtener un producto por su ID
        Sales ObtenerSalePorId(int saleId);

        // Método para actualizar información de un producto
        Sales ActualizarSale(int saleId, Sales saleActualizado);

        // Método para eliminar un producto
        bool EliminarSale(int saleId);
    }
}
