using DAL.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPIproject
{
    public class RestfulJWTL: IAuthInterface
    {
        private readonly IConfiguration _configuration;
       

        public RestfulJWTL(IConfiguration configuration)
        {
            _configuration = configuration;
           
        }
        public (string email, string password) DecodeJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var password = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                return (email, password);
            }
            catch
            {
                throw new SecurityTokenException("Invalid token");
            }
        }

    }
}
