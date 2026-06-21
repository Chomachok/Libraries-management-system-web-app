using LibrariesManagementSystem.Api.DTOs.Checkout;

namespace LibrariesManagementSystem.Api.Services.Interfaces;

/// <summary>
/// Сервис для управления выдачей и возвратом книг.
/// </summary>
public interface ICheckoutService
{
    /// <summary>
    /// Получить все выдачи (активные и завершённые) для библиотеки библиотекаря.
    /// </summary>
    /// <param name="librarianLibraryId">ID библиотеки.</param>
    /// <returns>Список выдач <see cref="CheckoutDto"/>.</returns>
    Task<List<CheckoutDto>> GetLibraryCheckouts(int librarianLibraryId);
    
    /// <summary>
    /// Получить активные выдачи конкретного читателя.
    /// </summary>
    /// <param name="userId">ID читателя.</param>
    /// <returns>Список активных выдач <see cref="CheckoutDto"/>.</returns>
    Task<List<CheckoutDto>> GetReaderActiveCheckouts(int userId);
    
    /// <summary>
    /// Получить историю всех выдач конкретного читателя (включая возвращённые).
    /// </summary>
    /// <param name="userId">ID читателя.</param>
    /// <returns>Список выдач <see cref="CheckoutDto"/>.</returns>
    Task<List<CheckoutDto>> GetReaderHistory(int userId);
    
    /// <summary>
    /// Читатель берёт книгу (самостоятельно).
    /// </summary>
    /// <param name="readerUserId">ID читателя.</param>
    /// <param name="bookId">ID книги.</param>
    /// <returns>Созданная запись выдачи <see cref="CheckoutDto"/>.</returns>
    Task<CheckoutDto> BorrowBook(int readerUserId, int bookId);
    
    /// <summary>
    /// Библиотекарь принимает возврат книги.
    /// </summary>
    /// <param name="librarianLibraryId">ID библиотеки.</param>
    /// <param name="checkoutId">ID записи выдачи.</param>
    /// <returns>Обновлённая запись выдачи <see cref="CheckoutDto"/> с датой возврата.</returns>
    Task<CheckoutDto> ReturnBook(int librarianLibraryId, int checkoutId);
    
    /// <summary>
    /// Читатель возвращает книгу (самостоятельно).
    /// </summary>
    /// <param name="userId">ID читателя.</param>
    /// <param name="checkoutId">ID записи выдачи.</param>
    /// <returns>Обновлённая запись выдачи <see cref="CheckoutDto"/>.</returns>
    Task<CheckoutDto> ReturnBookReader(int userId, int checkoutId);
    
    /// <summary>
    /// Библиотекарь выдаёт книгу читателю.
    /// </summary>
    /// <param name="librarianLibraryId">ID библиотеки.</param>
    /// <param name="dto">Данные для создания выдачи.</param>
    /// <returns>Созданная запись выдачи <see cref="CheckoutDto"/>.</returns>
    Task<CheckoutDto> IssueByLibrarian(int librarianLibraryId, CreateCheckoutDto dto);
}
