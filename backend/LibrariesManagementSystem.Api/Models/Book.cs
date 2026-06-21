namespace LibrariesManagementSystem.Api.Models;

/// <summary>
/// Модель книги в библиотеке. Содержит информацию о книге, количестве экземпляров и связях.
/// </summary>
public class Book
{
    /// <summary>
    /// Уникальный идентификатор книги.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Название книги.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Автор книги.
    /// </summary>
    public string Author { get; set; } = string.Empty;
    
    /// <summary>
    /// Международный стандартный книжный номер (ISBN).
    /// </summary>
    public string ISBN { get; set; } = string.Empty;
    
    /// <summary>
    /// Жанр книги.
    /// </summary>
    public string Genre { get; set; } = string.Empty;
    
    /// <summary>
    /// Год издания.
    /// </summary>
    public int Year { get; set; }
    
    /// <summary>
    /// Общее количество экземпляров данной книги в библиотеке.
    /// </summary>
    public int TotalCopies { get; set; }
    
    /// <summary>
    /// Количество экземпляров, доступных для выдачи в данный момент.
    /// </summary>
    public int AvailableCopies { get; set; }
    
    /// <summary>
    /// Идентификатор библиотеки, которой принадлежит книга.
    /// </summary>
    public int LibraryId { get; set; }
    
    /// <summary>
    /// Связанный объект библиотеки.
    /// </summary>
    public Library Library { get; set; } = null!;
    
    /// <summary>
    /// URL изображения обложки книги.
    /// </summary>
    public string? CoverImageUrl { get; set; }

    /// <summary>
    /// Список записей о выдаче книги читателям.
    /// </summary>
    public ICollection<Checkout> Checkouts { get; set; } = new List<Checkout>();
}
