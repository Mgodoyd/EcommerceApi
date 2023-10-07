using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface ILoginService
    {
        /*
         *     Métodos que debe tener la clase que implemente esta interfaz.
         *     
         *     Nota: Todos los métodos deben ser implementados en la clase que implemente esta interfaz para evitar
         *     algún error de implementación.
        */

        // Método para actualizar la password de un usuario
        Login UpdatePassword(string email, Login login);

        // Método para validar el usuario y la password
        string Authenticate(Login user, string plainPassword);

        // Método para generar el token
        string GenerateJwtToken(Login user);

        // Método para validar el token
        bool ValidateToken(string token);
    }
}
