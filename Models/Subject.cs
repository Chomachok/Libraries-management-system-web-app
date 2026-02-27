using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrariesWebApp.Models;

/// <summary>
/// Представляет тематическую рубрику (предметную область) книги.
/// </summary>
public partial class Subject
{
    /// <summary>
    /// Уникальный идентификатор тематической рубрики (первичный ключ).
    /// Автоматически генерируется базой данных.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SubjectId { get; set; }

    /// <summary>
    /// Название тематической рубрики (например, "Фантастика", "История", "Программирование").
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Навигационное свойство: коллекция книг, относящихся к данной тематической рубрике.
    /// </summary>
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}