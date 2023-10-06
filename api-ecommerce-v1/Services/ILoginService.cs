using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface ILoginService
    {
        Login UpdatePassword(string email, Login login);
        string Authenticate(Login user, string plainPassword);
        string GenerateJwtToken(Login user);
        bool ValidateToken(string token);
    }
}
