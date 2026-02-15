using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrariesWebApp.Models;

/// <summary>
/// Издательства
/// </summary>
public partial class Publisher
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PublisherId { get; set; }

    public string Name { get; set; } = null!;

    public string? City { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
