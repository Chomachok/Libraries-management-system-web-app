using System.ComponentModel.DataAnnotations;

namespace LibrariesManagementSystem.Api.DTOs.Book;

public class UpdateBookDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Author { get; set; } = string.Empty;

    public string ISBN { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int Year { get; set; }

    [Required, Range(1, int.MaxValue)]
    public int TotalCopies { get; set; }
    public string? CoverImageUrl { get; set; }
}