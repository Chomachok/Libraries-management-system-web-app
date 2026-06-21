using System.ComponentModel.DataAnnotations;

namespace LibrariesManagementSystem.Api.DTOs.Auth;

/// <summary>
/// Данные для регистрации нового пользователя.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Полное имя пользователя. Обязательное поле.
    /// </summary>
    [Required(ErrorMessage = "Имя обязательно")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Email пользователя. Обязательное поле, должно быть в формате email.
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Пароль пользователя. Обязательное поле, минимум 6 символов.
    /// </summary>
    [Required, MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор библиотеки, к которой будет привязан пользователь. Обязательное поле.
    /// </summary>
    [Required]
    public int LibraryId { get; set; }
}
