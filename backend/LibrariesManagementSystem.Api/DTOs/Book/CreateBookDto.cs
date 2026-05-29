using System.ComponentModel.DataAnnotations;

namespace LibrariesManagementSystem.Api.DTOs.Book;

public class CreateBookDto
{
    [Required(ErrorMessage = "Название обязательно")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Автор обязателен")]
    public string Author { get; set; } = string.Empty;

    public string ISBN { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int Year { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "Количество копий должно быть >= 1")]
    public int TotalCopies { get; set; }
}