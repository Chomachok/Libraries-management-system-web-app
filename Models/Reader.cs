using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrariesWebApp.Models;

/// <summary>
/// Представляет читателя библиотеки.
/// </summary>
public partial class Reader
{
    /// <summary>
    /// Уникальный идентификатор читателя (первичный ключ).
    /// Автоматически генерируется базой данных.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReaderId { get; set; }

    /// <summary>
    /// Полное имя читателя (фамилия, имя, отчество).
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Адрес проживания читателя (может отсутствовать).
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Контактный телефон читателя (может отсутствовать).
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Навигационное свойство: список выдач книг, оформленных на данного читателя.
    /// </summary>
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}