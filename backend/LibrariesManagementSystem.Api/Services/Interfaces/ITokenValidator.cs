using System.Security.Claims;

namespace LibrariesManagementSystem.Api.Services.Interfaces;

public interface ITokenValidator
{
    public ClaimsPrincipal?  ValidateToken(string token, string expectedType);
}