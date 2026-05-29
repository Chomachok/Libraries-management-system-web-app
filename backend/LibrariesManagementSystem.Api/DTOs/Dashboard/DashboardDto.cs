namespace LibrariesManagementSystem.Api.DTOs.Dashboard;

public class DashboardDto
{
    public int BooksCount { get; set; }
    public int ReadersCount { get; set; }
    public int ActiveCheckouts { get; set; }
    public int OverdueCheckouts { get; set; }
}