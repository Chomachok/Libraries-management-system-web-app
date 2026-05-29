using LibrariesManagementSystem.Api.DTOs.Dashboard;

namespace LibrariesManagementSystem.Api.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetLibraryStats(int librarianLibraryId);
}