namespace LibrariesManagementSystem.Api.DTOs.Dashboard;

/// <summary>
/// Сводная статистика библиотеки для панели управления.
/// </summary>
public class DashboardDto
{
    /// <summary>
    /// Общее количество книг в библиотеке.
    /// </summary>
    public int BooksCount { get; set; }
    
    /// <summary>
    /// Количество зарегистрированных читателей, привязанных к библиотеке.
    /// </summary>
    public int ReadersCount { get; set; }
    
    /// <summary>
    /// Количество активных выдач (книги находятся у читателей).
    /// </summary>
    public int ActiveCheckouts { get; set; }
    
    /// <summary>
    /// Количество просроченных выдач (книги не возвращены вовремя).
    /// </summary>
    public int OverdueCheckouts { get; set; }
}
