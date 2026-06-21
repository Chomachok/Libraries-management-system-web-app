namespace LibrariesManagementSystem.Api.Models;

/// <summary>
/// Модель библиотеки. Содержит информацию о названии, адресе, 
/// а также о связанных книгах и пользователях.
/// </summary>
public class Library
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
    
    /// <summary>
    /// Коллекция книг, принадлежащих библиотеке.
    /// </summary>
    public ICollection<Book> Books { get; set; } = new List<Book>();
    
    /// <summary>
    /// Коллекция пользователей (читателей и библиотекарей), привязанных к библиотеке.
    /// </summary>
    public ICollection<User> Users { get; set; } = new List<User>();
}
