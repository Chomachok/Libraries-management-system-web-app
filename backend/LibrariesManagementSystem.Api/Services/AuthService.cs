using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibrariesManagementSystem.Api.Data;
using LibrariesManagementSystem.Api.DTOs.Auth;
using LibrariesManagementSystem.Api.Models;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LibrariesManagementSystem.Api.Services;

public class AuthService(AppDbContext db, ITokenValidator tokenValidator, IConfiguration config) : IAuthService
{
    public async Task<(AuthResponseDto AuthResponse, string RefreshToken)> Register(RegisterDto dto)
    {
        if (await db.Users.AnyAsync(u => u.Email == dto.Email))
            throw new InvalidOperationException("Пользователь с таким email уже существует");

        var library = await db.Libraries.FindAsync(dto.LibraryId);
        if (library == null)
            throw new InvalidOperationException("Библиотека не найдена");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Reader,
            LibraryId = dto.LibraryId
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();
        
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken(user);

        var authResponse = new AuthResponseDto
        {
            Token = accessToken,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            UserId = user.Id,
            LibraryId = user.LibraryId
        };
        
        return (authResponse, refreshToken);
    }

    public async Task<(AuthResponseDto AuthResponse, string RefreshToken)> Login(LoginDto dto)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Неверный email или пароль");
        
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken(user);

        var authResponse = new AuthResponseDto
        {
            Token = accessToken,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            UserId = user.Id,
            LibraryId = user.LibraryId
        };
        
        return (authResponse, refreshToken);
    }

    public async Task<(AuthResponseDto? AuthResponse, string? RefreshToken)> RefreshAccessToken(string refreshToken)
    {
        // 1. Валидация refresh‑токена (подпись, срок, тип)
        var principal = tokenValidator.ValidateToken(refreshToken, "refresh");
        if (principal == null)
            return (null, null);

        // 2. Извлечение идентификатора пользователя
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return (null, null);

        // 3. Поиск пользователя в базе
        var user = await db.Users.FindAsync(userId);
        if (user == null)
            return (null, null);

        // 4. Генерация новой пары токенов
        var newAccessToken = GenerateAccessToken(user);
        var newRefreshToken = GenerateRefreshToken(user); // ротация refresh‑токена

        var authResponse = new AuthResponseDto
        {
            Token = newAccessToken,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            UserId = user.Id,
            LibraryId = user.LibraryId
        };

        return (authResponse, newRefreshToken);
    }

    private AuthResponseDto GenerateToken(User user)
    {
        var secret = config["JWT_SECRET"] ?? throw new Exception("JWT_SECRET not set");
        var issuer = config["JWT_ISSUER"] ?? "LibraryApi";
        var audience = config["JWT_AUDIENCE"] ?? "LibraryApp";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("LibraryId", user.LibraryId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            FullName = user.FullName,
            Role = user.Role.ToString(),
            UserId = user.Id,
            LibraryId = user.LibraryId
        };
    }
    
    // Генерация access‑токена (15 минут)
    private string GenerateAccessToken(User user)
    {
        return GenerateJwt(user, TimeSpan.FromMinutes(15), "access");
    }

// Генерация refresh‑токена (7 дней)
    private string GenerateRefreshToken(User user)
    {
        return GenerateJwt(user, TimeSpan.FromDays(7), "refresh");
    }

// Универсальный метод создания JWT
    private string GenerateJwt(User user, TimeSpan lifetime, string tokenType)
    {
        var secret = config["JWT_SECRET"];
        if (string.IsNullOrEmpty(secret))
            throw new InvalidOperationException("JWT_SECRET не настроен");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("LibraryId", user.LibraryId.ToString()),
            new Claim("tokenType", tokenType)   // различаем access и refresh
        };

        var token = new JwtSecurityToken(
            issuer: config["JWT_ISSUER"],
            audience: config["JWT_AUDIENCE"],
            claims: claims,
            expires: DateTime.UtcNow.Add(lifetime),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}