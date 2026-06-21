using System.ComponentModel.DataAnnotations;

namespace LibrariesManagementSystem.Api.DTOs.User;

/// <summary>
/// Данные для обновления профиля пользователя.
/// </summary>
public class UpdateUserDto
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
}
