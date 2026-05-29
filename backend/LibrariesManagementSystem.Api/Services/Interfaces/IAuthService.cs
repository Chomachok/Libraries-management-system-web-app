using LibrariesManagementSystem.Api.DTOs.Auth;

namespace LibrariesManagementSystem.Api.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> Register(RegisterDto dto);
    Task<AuthResponseDto> Login(LoginDto dto);
}