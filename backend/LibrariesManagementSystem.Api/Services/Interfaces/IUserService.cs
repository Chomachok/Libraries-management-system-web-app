using LibrariesManagementSystem.Api.DTOs.User;

namespace LibrariesManagementSystem.Api.Services.Interfaces;

/// <summary>
/// Сервис для управления пользователями (читателями) системы.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Получить список всех читателей, привязанных к библиотеке библиотекаря.
    /// </summary>
    /// <param name="librarianLibraryId">ID библиотеки.</param>
    /// <returns>Список пользователей <see cref="UserDto"/>.</returns>
    Task<List<UserDto>> GetReaders(int librarianLibraryId);
    
    /// <summary>
    /// Получить пользователя по идентификатору.
    /// </summary>
    /// <param name="userId">ID пользователя.</param>
    /// <returns>Данные пользователя <see cref="UserDto"/>.</returns>
    Task<UserDto> GetById(int userId);
    
    /// <summary>
    /// Создать нового читателя в библиотеке библиотекаря.
    /// </summary>
    /// <param name="librarianLibraryId">ID библиотеки.</param>
    /// <param name="dto">Данные для создания читателя.</param>
    /// <returns>Созданный пользователь <see cref="UserDto"/>.</returns>
    Task<UserDto> CreateReader(int librarianLibraryId, CreateUserDto dto);
    
    /// <summary>
    /// Обновить данные пользователя.
    /// </summary>
    /// <param name="userId">ID пользователя.</param>
    /// <param name="dto">Новые данные.</param>
    /// <returns>Обновлённый пользователь <see cref="UserDto"/>.</returns>
    Task<UserDto> Update(int userId, UpdateUserDto dto);
    
    /// <summary>
    /// Удалить пользователя.
    /// </summary>
    /// <param name="librarianLibraryId">ID библиотеки (для проверки принадлежности).</param>
    /// <param name="userId">ID удаляемого пользователя.</param>
    Task Delete(int librarianLibraryId, int userId);
    
    /// <summary>
    /// Обновить собственный профиль текущего пользователя.
    /// </summary>
    /// <param name="userId">ID пользователя.</param>
    /// <param name="dto">Новые данные профиля.</param>
    /// <returns>Обновлённый профиль <see cref="UserDto"/>.</returns>
    Task<UserDto> UpdateOwnProfile(int userId, UpdateUserDto dto);
}
