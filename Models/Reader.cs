using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrariesWebApp.Models;

/// <summary>
/// Читатели
/// </summary>
public partial class Reader
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReaderId { get; set; }

    /// <summary>
    /// ФИО полностью
    /// </summary>
    public string FullName { get; set; } = null!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
