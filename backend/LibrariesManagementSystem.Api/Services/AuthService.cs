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

public class AuthService(AppDbContext db, IConfiguration config) : IAuthService
{
    public async Task<AuthResponseDto> Register(RegisterDto dto)
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

        return GenerateToken(user);
    }

    public async Task<AuthResponseDto> Login(LoginDto dto)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Неверный email или пароль");

        return GenerateToken(user);
    }

    private AuthResponseDto GenerateToken(User user)
    {
        var secret = config["JWT_SECRET"]!;
        var issuer = config["JWT_ISSUER"]!;
        var audience = config["JWT_AUDIENCE"]!;

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
}