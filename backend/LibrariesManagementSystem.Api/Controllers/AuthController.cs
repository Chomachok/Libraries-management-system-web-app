using LibrariesManagementSystem.Api.DTOs.Auth;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesManagementSystem.Api.Controllers;

/// <summary>
/// Контроллер для аутентификации и управления токенами.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, IWebHostEnvironment env) : ControllerBase
{
    /// <summary>
    /// Регистрирует нового пользователя в системе.
    /// </summary>
    /// <param name="dto">Данные для регистрации: email, пароль, имя и т.д.</param>
    /// <returns>
    /// Объект <see cref="AuthResponseDto"/> с access-токеном и информацией о пользователе.
    /// Refresh-токен записывается в HttpOnly-куку.
    /// </returns>
    /// <response code="200">Успешная регистрация.</response>
    /// <response code="400">Некорректные данные (валидация) или пользователь уже существует.</response>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
    {
        var (authResponse, refreshToken) = await authService.Register(dto);
        SetRefreshTokenCookie(refreshToken);
        return Ok(authResponse);
    }
    
    /// <summary>
    /// Аутентифицирует пользователя по email и паролю.
    /// </summary>
    /// <param name="dto">Учетные данные: email и пароль.</param>
    /// <returns>Access-токен и информация о пользователе; refresh-токен в куке.</returns>
    /// <response code="200">Успешный вход.</response>
    /// <response code="401">Неверные учетные данные.</response>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var (authResponse, refreshToken) = await authService.Login(dto);
        SetRefreshTokenCookie(refreshToken);
        return Ok(authResponse);
    }
    
    /// <summary>
    /// Обновляет access-токен, используя refresh-токен из куки.
    /// </summary>
    /// <returns>Новый access-токен в теле ответа и обновлённый refresh-токен в куке.</returns>
    /// <response code="200">Токены успешно обновлены.</response>
    /// <response code="401">Refresh-токен отсутствует, невалиден или истёк.</response>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized("Refresh token not found");

        var (authResponse, newRefreshToken) = await authService.RefreshAccessToken(refreshToken);
        if (authResponse == null || newRefreshToken == null)
            return Unauthorized("Invalid refresh token");

        SetRefreshTokenCookie(newRefreshToken); // обновляем куку
        return Ok(new { accessToken = authResponse.Token });
    }
    
    /// <summary>
    /// Выходит из системы: удаляет refresh-токен из куки.
    /// </summary>
    /// <returns>Пустой ответ 200 OK.</returns>
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("refreshToken");
        return Ok();
    }

    /// <summary>
    /// Устанавливает refresh-токен в HttpOnly куку с настройками безопасности,
    /// зависящими от окружения (разработка/продакшен).
    /// </summary>
    /// <param name="token">Значение refresh-токена.</param>
    private void SetRefreshTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = !env.IsDevelopment(),
            SameSite = env.IsDevelopment() ? SameSiteMode.Lax : SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }
}
