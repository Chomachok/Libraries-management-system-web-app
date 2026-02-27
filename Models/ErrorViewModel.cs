namespace LibrariesWebApp.Models;

/// <summary>
/// Модель представления для страницы ошибки.
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Идентификатор запроса, связанный с ошибкой.
    /// Может использоваться для поиска в логах.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Определяет, нужно ли отображать идентификатор запроса.
    /// Возвращает <c>true</c>, если <see cref="RequestId"/> не равен <c>null</c> и не пуст.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}