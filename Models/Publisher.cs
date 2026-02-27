using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrariesWebApp.Models;

/// <summary>
/// Представляет издательство, выпускающее книги.
/// </summary>
public partial class Publisher
{
    /// <summary>
    /// Уникальный идентификатор издательства (первичный ключ).
    /// Автоматически генерируется базой данных.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PublisherId { get; set; }

    /// <summary>
    /// Название издательства.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Город, в котором находится издательство (может отсутствовать).
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Навигационное свойство: коллекция книг, выпущенных данным издательством.
    /// </summary>
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}