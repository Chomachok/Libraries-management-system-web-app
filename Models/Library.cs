using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrariesWebApp.Models;

/// <summary>
/// Филиалы библиотек
/// </summary>
public partial class Library
{
    /// <summary>
    /// Уникальный идентификатор библиотеки
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LibraryId { get; set; }

    /// <summary>
    /// Название библиотеки
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Адрес
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Контактный телефон
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Коллекция книг
    /// </summary>
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
