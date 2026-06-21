using LibrariesManagementSystem.Api.Data;
using LibrariesManagementSystem.Api.DTOs.Dashboard;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrariesManagementSystem.Api.Services;

/// <summary>
/// Реализация сервиса для получения сводной статистики панели управления библиотеки.
/// </summary>
public class DashboardService(AppDbContext db) : IDashboardService
{
    /// <summary>
    /// Получить ключевые показатели библиотеки: количество книг, читателей, активных и просроченных выдач.
    /// </summary>
    /// <param name="librarianLibraryId">ID библиотеки.</param>
    /// <returns>Объект <see cref="DashboardDto"/> со статистикой. Если библиотека не найдена, возвращается пустой объект.</returns>
    public async Task<DashboardDto> GetLibraryStats(int librarianLibraryId)
    {
        var now = DateTime.UtcNow;
        var stats = await db.Libraries
            .Where(l => l.Id == librarianLibraryId)
            .Select(l => new DashboardDto
            {
                BooksCount = l.Books.Count,
                ReadersCount = l.Users.Count(u => u.Role == Models.UserRole.Reader),
                ActiveCheckouts = l.Books.Sum(b => b.Checkouts.Count(c => c.ReturnDate == null)),
                OverdueCheckouts = l.Books.Sum(b => b.Checkouts.Count(c => c.ReturnDate == null && c.DueDate < now))
            })
            .FirstOrDefaultAsync();

        return stats ?? new DashboardDto();
    }
}
