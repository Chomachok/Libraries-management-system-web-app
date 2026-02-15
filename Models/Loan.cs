using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrariesWebApp.Models;

/// <summary>
/// Выдачи книг читателям
/// </summary>
public partial class Loan
{
    /// <summary>
    /// Уникальный номер записи о выдаче
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LoanId { get; set; }

    public int LibraryId { get; set; }

    public int BookCode { get; set; }

    public int ReaderId { get; set; }

    /// <summary>
    /// Дата выдачи
    /// </summary>
    public DateOnly? IssueDate { get; set; }

    /// <summary>
    /// Дата возврата (NULL, если ещё не возвращена)
    /// </summary>
    public DateOnly? ReturnDate { get; set; }

    /// <summary>
    /// Аванс (залог)
    /// </summary>
    public decimal? Advance { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Reader Reader { get; set; } = null!;
}
