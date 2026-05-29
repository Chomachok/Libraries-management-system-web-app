using LibrariesManagementSystem.Api.Data;
using LibrariesManagementSystem.Api.DTOs.User;
using LibrariesManagementSystem.Api.Models;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrariesManagementSystem.Api.Services;

public class UserService(AppDbContext db) : IUserService
{
    public async Task<List<UserDto>> GetReaders(int librarianLibraryId)
    {
        return await db.Users
            .Where(u => u.LibraryId == librarianLibraryId && u.Role == UserRole.Reader)
            .Select(u => MapToDto(u))
            .ToListAsync();
    }

    public async Task<UserDto> GetById(int userId)
    {
        var user = await db.Users.FindAsync(userId);
        if (user == null) throw new KeyNotFoundException("Пользователь не найден");
        return MapToDto(user);
    }

    public async Task<UserDto> CreateReader(int librarianLibraryId, CreateUserDto dto)
    {
        if (await db.Users.AnyAsync(u => u.Email == dto.Email))
            throw new InvalidOperationException("Email уже используется");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Reader,
            LibraryId = librarianLibraryId
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return MapToDto(user);
    }

    public async Task<UserDto> Update(int userId, UpdateUserDto dto)
    {
        var user = await db.Users.FindAsync(userId);
        if (user == null) throw new KeyNotFoundException("Пользователь не найден");

        if (await db.Users.AnyAsync(u => u.Email == dto.Email && u.Id != userId))
            throw new InvalidOperationException("Email уже используется другим пользователем");

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        await db.SaveChangesAsync();

        return MapToDto(user);
    }

    public async Task Delete(int librarianLibraryId, int userId)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId && u.LibraryId == librarianLibraryId && u.Role == UserRole.Reader);
        if (user == null) throw new KeyNotFoundException("Читатель не найден");
        db.Users.Remove(user);
        await db.SaveChangesAsync();
    }

    public async Task<UserDto> UpdateOwnProfile(int userId, UpdateUserDto dto)
    {
        var updatedUser = await Update(userId, dto);
        return updatedUser;
    }

    private static UserDto MapToDto(User u) => new()
    {
        Id = u.Id,
        FullName = u.FullName,
        Email = u.Email,
        Role = u.Role.ToString(),
        LibraryId = u.LibraryId
    };
}