namespace LibrariesManagementSystem.Api.DTOs.Library;

/// <summary>
/// Информация о библиотеке.
/// </summary>
public class LibraryDto
{
    /// <summary>
    /// Уникальный идентификатор библиотеки.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Название библиотеки.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Адрес библиотеки.
    /// </summary>
    public string Address { get; set; } = string.Empty;
}
