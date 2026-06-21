namespace LibrariesManagementSystem.Api.Models;

/// <summary>
/// Роль пользователя в системе.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Читатель.
    /// </summary>
    Reader,
    
    /// <summary>
    /// Библиотекарь.
    /// </summary>
    Librarian
}

/// <summary>
/// Модель пользователя системы (читатель или библиотекарь).
/// </summary>
public class User
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
    /// Email пользователя (уникальный).
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Хеш пароля пользователя.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;
    
    /// <summary>
    /// Роль пользователя в системе.
    /// </summary>
    public UserRole Role { get; set; }
    
    /// <summary>
    /// Идентификатор библиотеки, к которой привязан пользователь.
    /// </summary>
    public int LibraryId { get; set; }
    
    /// <summary>
    /// Связанный объект библиотеки.
    /// </summary>
    public Library Library { get; set; } = null!;

    /// <summary>
    /// Список записей о выдаче книг этому пользователю.
    /// </summary>
    public ICollection<Checkout> Checkouts { get; set; } = new List<Checkout>();
}
