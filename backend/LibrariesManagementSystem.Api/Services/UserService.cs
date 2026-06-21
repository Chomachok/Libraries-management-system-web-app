using LibrariesManagementSystem.Api.Data;
using LibrariesManagementSystem.Api.DTOs.User;
using LibrariesManagementSystem.Api.Models;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrariesManagementSystem.Api.Services;

/// <summary>
/// Реализация сервиса для управления пользователями (читателями).
/// Позволяет библиотекарям просматривать, создавать, редактировать и удалять читателей своей библиотеки,
/// а также пользователям управлять собственным профилем./// </summary>
public class UserService(AppDbContext db) : IUserService
{
    /// <summary>
    /// Получить список всех читателей, привязанных к указанной библиотеке.
    /// </summary>
    /// <param name="librarianLibraryId">ID библиотеки.</param>
    /// <returns>Список пользователей <see cref="UserDto"/> с ролью Reader.</returns>
    public async Task<List<UserDto>> GetReaders(int librarianLibraryId)
    {
        return await db.Users
            .Where(u => u.LibraryId == librarianLibraryId && u.Role == UserRole.Reader)
            .Select(u => MapToDto(u))
            .ToListAsync();
    }

    /// <summary>
    /// Получить пользователя по идентификатору (любая роль).
    /// </summary>
    /// <param name="userId">ID пользователя.</param>
    /// <returns>Данные пользователя <see cref="UserDto"/>.</returns>
    /// <exception cref="KeyNotFoundException">Пользователь с указанным ID не найден.</exception>
    public async Task<UserDto> GetById(int userId)
    {
        var user = await db.Users.FindAsync(userId);
        if (user == null) throw new KeyNotFoundException("Пользователь не найден");
        return MapToDto(user);
    }

    /// <summary>
    /// Создать нового читателя в библиотеке библиотекаря. Пароль хешируется с помощью BCrypt.
    /// </summary>
    /// <param name="librarianLibraryId">ID библиотеки, к которой привязывается читатель.</param>
    /// <param name="dto">Данные нового читателя (имя, email, пароль).</param>
    /// <returns>Созданный пользователь <see cref="UserDto"/>.</returns>
    /// <exception cref="InvalidOperationException">Email уже используется.</exception>
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

    /// <summary>
    /// Обновить данные пользователя (имя и email) по идентификатору.
    /// Используется библиотекарем или для обновления собственного профиля.
    /// </summary>
    /// <param name="userId">ID обновляемого пользователя.</param>
    /// <param name="dto">Новые данные (имя, email).</param>
    /// <returns>Обновлённый пользователь <see cref="UserDto"/>.</returns>
    /// <exception cref="KeyNotFoundException">Пользователь не найден.</exception>
    /// <exception cref="InvalidOperationException">Новый email уже используется другим пользователем.</exception>
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

    /// <summary>
    /// Удалить читателя из библиотеки. Удаление возможно только для пользователей с ролью Reader.
    /// </summary>
    /// <param name="librarianLibraryId">ID библиотеки.</param>
    /// <param name="userId">ID удаляемого читателя.</param>
    /// <exception cref="KeyNotFoundException">Читатель не найден в указанной библиотеке или его роль не Reader.</exception>
    public async Task Delete(int librarianLibraryId, int userId)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId && u.LibraryId == librarianLibraryId && u.Role == UserRole.Reader);
        if (user == null) throw new KeyNotFoundException("Читатель не найден");
        db.Users.Remove(user);
        await db.SaveChangesAsync();
    }

    /// <summary>
    /// Обновить собственный профиль текущего пользователя. Вызывает общий метод <see cref="Update"/>.
    /// </summary>
    /// <param name="userId">ID текущего пользователя.</param>
    /// <param name="dto">Новые данные профиля.</param>
    /// <returns>Обновлённый профиль <see cref="UserDto"/>.</returns>
    public async Task<UserDto> UpdateOwnProfile(int userId, UpdateUserDto dto)
    {
        var updatedUser = await Update(userId, dto);
        return updatedUser;
    }

    /// <summary>
    /// Преобразует сущность <see cref="User"/> в DTO <see cref="UserDto"/> для передачи клиенту.
    /// </summary>
    private static UserDto MapToDto(User u) => new()
    {
        Id = u.Id,
        FullName = u.FullName,
        Email = u.Email,
        Role = u.Role.ToString(),
        LibraryId = u.LibraryId
    };
}
