namespace LibrariesManagementSystem.Api.DTOs.Checkout;

/// <summary>
/// Модель записи о выдаче/возврате книги.
/// </summary>
public class CheckoutDto
{
    /// <summary>
    /// Уникальный идентификатор записи выдачи.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Идентификатор читателя, взявшего книгу.
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Имя читателя.
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    
    /// <summary>
    /// Идентификатор взятой книги.
    /// </summary>
    public int BookId { get; set; }
    
    /// <summary>
    /// Название книги.
    /// </summary>
    public string BookTitle { get; set; } = string.Empty;
    
    /// <summary>
    /// Дата и время выдачи книги.
    /// </summary>
    public DateTime CheckoutDate { get; set; }
    
    /// <summary>
    /// Дата, до которой книгу нужно вернуть.
    /// </summary>
    public DateTime DueDate { get; set; }
    
    /// <summary>
    /// Фактическая дата возврата книги (null, если ещё не возвращена).
    /// </summary>
    public DateTime? ReturnDate { get; set; }
    
    /// <summary>
    /// Сумма штрафа за просрочку (null, если штрафа нет или книга ещё не возвращена).
    /// </summary>
    public decimal? FineAmount { get; set; }
}
