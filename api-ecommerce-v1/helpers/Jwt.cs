using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace api_ecommerce_v1.helpers
{
    public class Jwt
    {
        public string Key { get;  set; }
        public string Subject { get;  set; }
        public int ExpiresInMinutes { get;  set; }
    }
}
