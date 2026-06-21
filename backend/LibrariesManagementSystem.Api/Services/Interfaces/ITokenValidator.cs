using System.Security.Claims;

namespace LibrariesManagementSystem.Api.Services.Interfaces;

/// <summary>
/// Сервис валидации JWT-токенов.
/// </summary>
public interface ITokenValidator
{
    /// <summary>
    /// Проверяет токен на подлинность, срок действия и соответствие ожидаемому типу.
    /// </summary>
    /// <param name="token">Строка проверяемого токена.</param>
    /// <param name="expectedType">Ожидаемый тип токена (например, "access" или "refresh").</param>
    /// <returns>
    /// Объект <see cref="ClaimsPrincipal"/> с утверждениями (claims) пользователя, если токен валиден;
    /// в противном случае - <c>null</c>.
    /// </returns>
    public ClaimsPrincipal?  ValidateToken(string token, string expectedType);
}
