namespace LibrariesManagementSystem.Api.DTOs.Auth;

/// <summary>
/// Ответ после успешной аутентификации или регистрации.
/// Содержит информацию о пользователе и выданные токены.
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// Токен доступа (access-токен).
    /// </summary>
    public string Token { get; set; } = string.Empty;
    
    /// <summary>
    /// Токен обновления (refresh-токен).
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
    
    /// <summary>
    /// Альтернативное поле для access-токена (используется в некоторых методах).
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;
    
    /// <summary>
    /// Полное имя пользователя.
    /// </summary>
    public string FullName { get; set; } = string.Empty;
    
    /// <summary>
    /// Роль пользователя (например, "Reader", "Librarian").
    /// </summary>
    public string Role { get; set; } = string.Empty;
    
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Идентификатор библиотеки, к которой привязан пользователь.
    /// </summary>
    public int LibraryId { get; set; }
}
