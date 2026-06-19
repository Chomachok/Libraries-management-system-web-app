namespace LibrariesManagementSystem.Api.DTOs.Checkout;

/// <summary>
/// Данные для создания новой выдачи книги библиотекарем.
/// </summary>
public class CreateCheckoutDto
{
    /// <summary>
    /// Идентификатор читателя, которому выдаётся книга.
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Идентификатор книги, которая выдаётся.
    /// </summary>
    public int BookId { get; set; }
    
    /// <summary>
    /// Срок выдачи в днях. Значение по умолчанию - 14 дней.
    /// </summary>
    public int DurationDays { get; set; } = 14;
}
