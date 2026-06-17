using LibrariesManagementSystem.Api.DTOs.Dashboard;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesManagementSystem.Api.Controllers;

/// <summary>
/// Контроллер для получения статистики и данных панели управления библиотекой.
/// Все методы доступны только пользователям с ролью Librarian.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Librarian")]
public class DashboardController(IDashboardService dashboardService) : ControllerBase
{
    /// <summary>
    /// Получить сводную статистику для панели управления библиотекой текущего сотрудника.
    /// </summary>
    /// <returns>Объект <see cref="DashboardDto"/> с ключевыми показателями библиотеки.</returns>
    /// <response code="200">Статистика успешно получена.</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    /// <response code="403">Недостаточно прав (требуется роль Librarian).</response>
    [HttpGet("stats")]
    public async Task<ActionResult<DashboardDto>> GetStats()
    {
        var libId = int.Parse(User.FindFirst("LibraryId")!.Value);
        var stats = await dashboardService.GetLibraryStats(libId);
        return Ok(stats);
    }
}