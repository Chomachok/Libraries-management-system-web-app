using LibrariesManagementSystem.Api.Data;
using LibrariesManagementSystem.Api.DTOs.Library;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrariesManagementSystem.Api.Services;

/// <summary>
/// Реализация сервиса для получения списка библиотек.
/// </summary>
public class LibraryService(AppDbContext db) : ILibraryService
{
    /// <summary>
    /// Получить список всех библиотек системы.
    /// </summary>
    /// <returns>Список объектов <see cref="LibraryDto"/>.</returns>
    public async Task<List<LibraryDto>> GetAll()
    {
        return await db.Libraries
            .Select(l => new LibraryDto
            {
                Id = l.Id,
                Name = l.Name,
                Address = l.Address
            })
            .ToListAsync();
    }
}
