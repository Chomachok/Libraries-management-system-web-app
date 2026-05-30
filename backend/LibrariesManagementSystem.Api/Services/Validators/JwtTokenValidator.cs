using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace LibrariesManagementSystem.Api.Services.Validators;

public class JwtTokenValidator(IConfiguration config) : ITokenValidator
{
    public ClaimsPrincipal? ValidateToken(string token, string expectedTokenType)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = config["JWT_SECRET"];
            if (string.IsNullOrEmpty(secret))
                throw new InvalidOperationException("JWT_SECRET не настроен");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,   // проверяет истечение срока
                ValidateIssuerSigningKey = true,
                ValidIssuer = config["JWT_ISSUER"],
                ValidAudience = config["JWT_AUDIENCE"],
                IssuerSigningKey = key
            };

            var principal = tokenHandler.ValidateToken(token, parameters, out _);

            // Дополнительно убедимся, что это именно refresh‑токен
            var tokenTypeClaim = principal.FindFirst("tokenType")?.Value;
            if (tokenTypeClaim != expectedTokenType)
                return null;

            return principal;
        }
        catch
        {
            return null;
        }
    }
}