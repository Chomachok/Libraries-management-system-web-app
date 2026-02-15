using System;
using System.Collections.Generic;

namespace LibrariesWebApp.Models;

/// <summary>
/// Экземпляры книг в библиотеках
/// </summary>
public partial class Book
{
    /// <summary>
    /// Код библиотеки (часть составного ключа)
    /// </summary>
    public int LibraryId { get; set; }

    /// <summary>
    /// Внутренний код книги в библиотеке
    /// </summary>
    public int BookCode { get; set; }

    /// <summary>
    /// Тематика книги
    /// </summary>
    public int? SubjectId { get; set; }

    /// <summary>
    /// Издательство
    /// </summary>
    public int? PublisherId { get; set; }

    /// <summary>
    /// Автор
    /// </summary>
    public string Author { get; set; } = null!;

    /// <summary>
    /// Название
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Год издания
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// Количество экземпляров в данной библиотеке
    /// </summary>
    public int? Quantity { get; set; }

    public virtual Library Library { get; set; } = null!;

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public virtual Publisher? Publisher { get; set; }

    public virtual Subject? Subject { get; set; }
}
