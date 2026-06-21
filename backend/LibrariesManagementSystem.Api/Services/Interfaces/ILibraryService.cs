using LibrariesManagementSystem.Api.DTOs.Library;

namespace LibrariesManagementSystem.Api.Services.Interfaces;

/// <summary>
/// Сервис для получения информации о библиотеках.
/// </summary>
public interface ILibraryService
{
    /// <summary>
    /// Получить список всех библиотек системы.
    /// </summary>
    /// <returns>Список объектов <see cref="LibraryDto"/>.</returns>
    Task<List<LibraryDto>> GetAll();
}
