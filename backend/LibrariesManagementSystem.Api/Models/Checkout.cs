namespace LibrariesManagementSystem.Api.Models;

/// <summary>
/// Модель записи о выдаче книги читателю.
/// Хранит информацию о том, кто и когда взял книгу, срок возврата и возможный штраф.
/// </summary>
public class Checkout
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
    /// Связанный объект читателя.
    /// </summary>
    public User User { get; set; } = null!;
    
    /// <summary>
    /// Идентификатор выданной книги.
    /// </summary>
    public int BookId { get; set; }
    
    /// <summary>
    /// Связанный объект книги.
    /// </summary>
    public Book Book { get; set; } = null!;
    
    /// <summary>
    /// Дата и время выдачи книги.
    /// </summary>
    public DateTime CheckoutDate { get; set; }
    
    /// <summary>
    /// Дата, до которой необходимо вернуть книгу.
    /// </summary>
    public DateTime DueDate { get; set; }
    
    /// <summary>
    /// Фактическая дата возврата книги (null, если книга ещё не возвращена).
    /// </summary>
    public DateTime? ReturnDate { get; set; }
    
    /// <summary>
    /// Сумма штрафа за просрочку возврата (null, если штрафа нет или книга ещё не возвращена).
    /// </summary>
    public decimal? FineAmount { get; set; }
}
