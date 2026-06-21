namespace LibrariesManagementSystem.Api.DTOs.User;

/// <summary>
/// Информация о пользователе системы (читателе или библиотекаре).
/// </summary>
public class UserDto
{
    /// <summary>
    /// Уникальный идентификатор пользователя.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Полное имя пользователя.
    /// </summary>
    public string FullName { get; set; } = string.Empty;
    
    /// <summary>
    /// Email пользователя.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Роль пользователя (например, "Reader" или "Librarian").
    /// </summary>
    public string Role { get; set; } = string.Empty;
    
    /// <summary>
    /// Идентификатор библиотеки, к которой привязан пользователь.
    /// </summary>
    public int LibraryId { get; set; }
}
