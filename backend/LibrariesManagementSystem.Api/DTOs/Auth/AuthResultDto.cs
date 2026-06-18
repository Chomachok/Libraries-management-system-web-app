namespace LibrariesManagementSystem.Api.DTOs.Auth;

/// <summary>
/// Внутренняя модель, возвращаемая сервисом аутентификации.
/// Содержит сгенерированную пару токенов (access и refresh).
/// </summary>
public class AuthResultDto
{
    /// <summary>
    /// Токен доступа (access-токен).
    /// </summary>
    public string AccessToken { get; set; }
    
    /// <summary>
    /// Токен обновления (refresh-токен).
    /// </summary>
    public string RefreshToken { get; set; }
}
