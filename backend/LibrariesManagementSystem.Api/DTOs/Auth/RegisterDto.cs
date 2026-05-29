using System.ComponentModel.DataAnnotations;

namespace LibrariesManagementSystem.Api.DTOs.Auth;

public class RegisterDto
{
    [Required(ErrorMessage = "Имя обязательно")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
    public string Password { get; set; } = string.Empty;

    [Required]
    public int LibraryId { get; set; }
}