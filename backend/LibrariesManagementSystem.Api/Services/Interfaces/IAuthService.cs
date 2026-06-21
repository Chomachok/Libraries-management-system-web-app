using LibrariesManagementSystem.Api.DTOs.Auth;

namespace LibrariesManagementSystem.Api.Services.Interfaces;

/// <summary>
/// Сервис аутентификации и управления токенами доступа.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Регистрирует нового пользователя и возвращает пару токенов.
    /// </summary>
    /// <param name="dto">Данные для регистрации.</param>
    /// <returns>Кортеж: объект <see cref="AuthResponseDto"/> с данными пользователя и access-токеном, и refresh-токен.</returns>
    Task<(AuthResponseDto AuthResponse, string RefreshToken)> Register(RegisterDto dto);
    
    /// <summary>
    /// Аутентифицирует пользователя по email и паролю и возвращает пару токенов.
    /// </summary>
    /// <param name="dto">Учетные данные для входа.</param>
    /// <returns>Кортеж: объект <see cref="AuthResponseDto"/> с данными пользователя и access-токеном, и refresh-токен.</returns>
    Task<(AuthResponseDto AuthResponse, string RefreshToken)> Login(LoginDto dto);
    
    /// <summary>
    /// Обновляет пару токенов по действующему refresh-токену.
    /// </summary>
    /// <param name="refreshToken">Текущий refresh-токен.</param>
    /// <returns>Кортеж из нового <see cref="AuthResponseDto"/> (с новым access-токеном) и нового refresh-токена, 
    /// либо null-значения, если refresh-токен недействителен или истёк.</returns>
    Task<(AuthResponseDto? AuthResponse, string? RefreshToken)> RefreshAccessToken(string refreshToken);
}
