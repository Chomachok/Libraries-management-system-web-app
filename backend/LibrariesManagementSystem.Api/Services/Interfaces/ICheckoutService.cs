using LibrariesManagementSystem.Api.DTOs.Checkout;

namespace LibrariesManagementSystem.Api.Services.Interfaces;

public interface ICheckoutService
{
    Task<List<CheckoutDto>> GetLibraryCheckouts(int librarianLibraryId);
    Task<List<CheckoutDto>> GetReaderActiveCheckouts(int userId);
    Task<List<CheckoutDto>> GetReaderHistory(int userId);
    Task<CheckoutDto> BorrowBook(int readerUserId, int bookId);
    Task<CheckoutDto> ReturnBook(int librarianLibraryId, int checkoutId);
    Task<CheckoutDto> ReturnBookReader(int userId, int checkoutId);
    Task<CheckoutDto> IssueByLibrarian(int librarianLibraryId, CreateCheckoutDto dto);
}