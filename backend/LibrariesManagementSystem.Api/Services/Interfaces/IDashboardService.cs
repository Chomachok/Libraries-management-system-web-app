using LibrariesManagementSystem.Api.DTOs.Dashboard;

namespace LibrariesManagementSystem.Api.Services.Interfaces;

/// <summary>
/// Сервис для получения статистики панели управления библиотекой.
/// </summary>
public interface IDashboardService
{
    /// <summary>
    /// Получить сводную статистику по библиотеке (книги, читатели, выдачи, просрочки).
    /// </summary>
    /// <param name="librarianLibraryId">ID библиотеки.</param>
    /// <returns>Объект <see cref="DashboardDto"/> с ключевыми показателями.</returns>
    Task<DashboardDto> GetLibraryStats(int librarianLibraryId);
}
