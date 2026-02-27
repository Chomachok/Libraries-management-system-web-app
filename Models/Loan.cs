using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrariesWebApp.Models;

/// <summary>
/// Представляет запись о выдаче книги читателю в библиотеке.
/// Содержит информацию о дате выдачи, возврата, а также связи с книгой и читателем.
/// </summary>
public partial class Loan
{
    /// <summary>
    /// Уникальный идентификатор записи о выдаче (первичный ключ).
    /// Автоматически генерируется базой данных.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LoanId { get; set; }

    /// <summary>
    /// Идентификатор библиотеки, в которой произведена выдача.
    /// Входит в состав внешнего ключа к сущности <see cref="Book"/>.
    /// </summary>
    public int LibraryId { get; set; }
    
    /// <summary>
    /// Внутренний код книги в библиотеке.
    /// Входит в состав внешнего ключа к сущности <see cref="Book"/>.
    /// </summary>
    public int BookCode { get; set; }

    /// <summary>
    /// Идентификатор читателя, получившего книгу.
    /// Внешний ключ к сущности <see cref="Reader"/>.
    /// </summary>
    public int ReaderId { get; set; }

    /// <summary>
    /// Дата выдачи книги читателю.
    /// </summary>
    public DateOnly? IssueDate { get; set; }

    /// <summary>
    /// Дата возврата книги в библиотеку.
    /// Значение <c>null</c> означает, что книга ещё не возвращена.
    /// </summary>
    public DateOnly? ReturnDate { get; set; }

    /// <summary>
    /// Сумма аванса (залога), внесённого читателем при выдаче.
    /// Может быть <c>null</c>, если залог не требуется.
    /// </summary>
    public decimal? Advance { get; set; }

    /// <summary>
    /// Навигационное свойство: книга, которая была выдана.
    /// </summary>
    public virtual Book Book { get; set; } = null!;

    /// <summary>
    /// Навигационное свойство: читатель, получивший книгу.
    /// </summary>
    public virtual Reader Reader { get; set; } = null!;
}