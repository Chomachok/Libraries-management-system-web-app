using LibrariesManagementSystem.Api.DTOs.Auth;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, IWebHostEnvironment env) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
    {
        var (authResponse, refreshToken) = await authService.Register(dto);
        SetRefreshTokenCookie(refreshToken);
        return Ok(authResponse);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (authResponse, refreshToken) = await authService.Login(dto);
        SetRefreshTokenCookie(refreshToken);
        return Ok(authResponse);
    }
    
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

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("refreshToken");
        return Ok();
    }

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