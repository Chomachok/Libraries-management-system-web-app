using System.ComponentModel.DataAnnotations;

namespace LibrariesManagementSystem.Api.DTOs.User;

/// <summary>
/// Данные для создания нового пользователя (читателя) библиотекарем.
/// </summary>
public class CreateUserDto
{
    /// <summary>
    /// Полное имя пользователя. Обязательное поле.
    /// </summary>
    [Required]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Email пользователя. Обязательное поле, должно быть в формате email.
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Пароль пользователя. Обязательное поле, минимум 6 символов.
    /// </summary>
    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;
}
