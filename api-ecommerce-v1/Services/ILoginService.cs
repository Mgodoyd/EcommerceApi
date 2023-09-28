using api_ecommerce_v1.Models;

namespace api_ecommerce_v1.Services
{
    public interface ILoginService
    {
        string Authenticate(Login user, string plainPassword);
        string GenerateJwtToken(Login user);
        bool ValidateToken(string token);
    }
}
