using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface ISale
    {
        /*
         *     Métodos que debe tener la clase que implemente esta interfaz.
         *     
         *     Nota: Todos los métodos deben ser implementados en la clase que implemente esta interfaz para evitar
         *     algún error de implementación.
        */

        // Método para crear un nuevo venta
        Sales CrearSale(Sales address);

        // Método para obtener todos las ventas
        List<Sales> ObtenerTodoslasSale();

        // Método para obtener todos las ventas por usuario
        List<Sales> ObtenerVentasPorUserId(int userId);

        // Método para obtener el total de ventas estado entregado
        int ObtenerTotaldeSalesVendido();

        // Método para obtener el total de ventas cualquier estado
        int ObtenerTotaldeSalesGeneral();

        //Método para obtener el total de ventas estado cancelado
        int ObtenerTotaldeSalesCancelado();

        // Método para obtener la cantidad de ventas totales
        int ObtenerTotaldeSalesTotal(); 

        // Método para obtener una venta por su ID
        Sales ObtenerSalePorId(int saleId);

        // Método para actualizar información de una venta
        Sales ActualizarSale(int saleId, Sales saleActualizado);

        // Método para eliminar una venta
        bool EliminarSale(int saleId);
    }
}
