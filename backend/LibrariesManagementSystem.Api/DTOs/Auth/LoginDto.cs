using System.ComponentModel.DataAnnotations;

namespace LibrariesManagementSystem.Api.DTOs.Auth;

/// <summary>
/// Данные для входа пользователя в систему.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// Email пользователя. Обязательное поле, должно быть в формате email.
    /// </summary>
    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Пароль пользователя. Обязательное поле.
    /// </summary>
    [Required(ErrorMessage = "Пароль обязателен")]
    public string Password { get; set; } = string.Empty;
}
