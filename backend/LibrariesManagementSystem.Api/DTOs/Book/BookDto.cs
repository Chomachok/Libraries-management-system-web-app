namespace LibrariesManagementSystem.Api.DTOs.Book;

/// <summary>
/// Модель книги, используемая для отображения информации в API.
/// </summary>
public class BookDto
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
    /// Общее количество экземпляров книги в библиотеке.
    /// </summary>
    public int TotalCopies { get; set; }
    
    /// <summary>
    /// Количество доступных для выдачи экземпляров.
    /// </summary>
    public int AvailableCopies { get; set; }
    
    /// <summary>
    /// Идентификатор библиотеки, в которой находится книга.
    /// </summary>
    public int LibraryId { get; set; }
    
    /// <summary>
    /// Название библиотеки.
    /// </summary>
    public string LibraryName { get; set; } = string.Empty;
    
    /// <summary>
    /// URL изображения обложки книги (может отсутствовать).
    /// </summary>
    public string? CoverImageUrl { get; set; }
}
