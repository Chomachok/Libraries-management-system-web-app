using System.ComponentModel.DataAnnotations;

namespace LibrariesManagementSystem.Api.DTOs.Book;

/// <summary>
/// Данные для обновления существующей книги.
/// </summary>
public class UpdateBookDto
{
    /// <summary>
    /// Название книги. Обязательное поле.
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Автор книги. Обязательное поле.
    /// </summary>
    [Required]
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Международный стандартный книжный номер (ISBN). Необязательное поле.
    /// </summary>
    public string ISBN { get; set; } = string.Empty;
    
    /// <summary>
    /// Жанр книги. Необязательное поле.
    /// </summary>
    public string Genre { get; set; } = string.Empty;
    
    /// <summary>
    /// Год издания. Необязательное поле.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Общее количество экземпляров книги. Обязательное поле, должно быть не менее 1.
    /// </summary>
    [Required, Range(1, int.MaxValue)]
    public int TotalCopies { get; set; }
    
    /// <summary>
    /// URL изображения обложки книги. Необязательное поле.
    /// </summary>
    public string? CoverImageUrl { get; set; }
}
