using LibrariesManagementSystem.Api.DTOs.Dashboard;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Librarian")]
public class DashboardController(IDashboardService dashboardService) : ControllerBase
{
    [HttpGet("stats")]
    public async Task<ActionResult<DashboardDto>> GetStats()
    {
        var libId = int.Parse(User.FindFirst("LibraryId")!.Value);
        var stats = await dashboardService.GetLibraryStats(libId);
        return Ok(stats);
    }
}