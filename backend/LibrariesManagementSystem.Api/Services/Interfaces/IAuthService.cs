using LibrariesManagementSystem.Api.DTOs.Auth;

namespace LibrariesManagementSystem.Api.Services.Interfaces;

public interface IAuthService
{
    Task<(AuthResponseDto AuthResponse, string RefreshToken)> Register(RegisterDto dto);
    Task<(AuthResponseDto AuthResponse, string RefreshToken)> Login(LoginDto dto);
    Task<(AuthResponseDto? AuthResponse, string? RefreshToken)> RefreshAccessToken(string refreshToken);
}