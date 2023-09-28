using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using api_ecommerce_v1.Services;

namespace api_ecommerce_v1.helpers
{
    public class JwtAuthorizationFilter : IAuthorizationFilter
    {
        private readonly ILoginService _loginService;

        public JwtAuthorizationFilter(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authorizationHeader = context.HttpContext.Request.Headers.ContainsKey("Authorization")
                ? context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()
                : null;

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    message = "No tienes permisos para realizar esta acción"
                });
                return;
            }

            var token = authorizationHeader.Split(" ").Last();

            if (!_loginService.ValidateToken(token))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    message = "Token JWT no válido o no eres Admin"
                });
                return;
            }
            Console.WriteLine("Autorización exitosa.");
        }
    }
}
