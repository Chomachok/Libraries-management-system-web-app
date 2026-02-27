namespace LibrariesWebApp.Models;

/// <summary>
/// Представляет экземпляр книги, находящейся в конкретной библиотеке.
/// Каждая запись соответствует одной книге (название, автор) с указанием количества экземпляров в библиотеке.
/// </summary>
public partial class Book
{
    /// <summary>
    /// Код библиотеки (часть составного ключа).
    /// </summary>
    public int LibraryId { get; set; }

    /// <summary>
    /// Внутренний код книги в библиотеке (часть составного ключа).
    /// </summary>
    public int BookCode { get; set; }

    /// <summary>
    /// Идентификатор тематики (предметной области) книги.
    /// </summary>
    public int? SubjectId { get; set; }

    /// <summary>
    /// Идентификатор издательства.
    /// </summary>
    public int? PublisherId { get; set; }

    /// <summary>
    /// Автор книги.
    /// </summary>
    public string Author { get; set; } = null!;

    /// <summary>
    /// Название книги.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Год издания книги.
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// Количество экземпляров данной книги в библиотеке.
    /// </summary>
    public int? Quantity { get; set; }

    /// <summary>
    /// Навигационное свойство: библиотека, в которой находится книга.
    /// </summary>
    public virtual Library Library { get; set; } = null!;

    /// <summary>
    /// Коллекция выдач (записей о выдаче книги читателям).
    /// </summary>
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    /// <summary>
    /// Навигационное свойство: издательство книги.
    /// </summary>
    public virtual Publisher? Publisher { get; set; }

    /// <summary>
    /// Навигационное свойство: тематика (предметная область) книги.
    /// </summary>
    public virtual Subject? Subject { get; set; }
}