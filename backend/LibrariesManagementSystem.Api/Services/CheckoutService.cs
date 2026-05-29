using LibrariesManagementSystem.Api.Data;
using LibrariesManagementSystem.Api.DTOs.Checkout;
using LibrariesManagementSystem.Api.Models;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrariesManagementSystem.Api.Services;

public class CheckoutService : ICheckoutService
{
    private readonly AppDbContext _db;

    public CheckoutService(AppDbContext db) => _db = db;

    public async Task<List<CheckoutDto>> GetLibraryCheckouts(int librarianLibraryId)
    {
        return await _db.Checkouts
            .Include(c => c.Book)
            .Include(c => c.User)
            .Where(c => c.Book.LibraryId == librarianLibraryId)
            .OrderByDescending(c => c.CheckoutDate)
            .Select(c => MapToDto(c))
            .ToListAsync();
    }

    public async Task<List<CheckoutDto>> GetReaderActiveCheckouts(int userId)
    {
        return await _db.Checkouts
            .Include(c => c.Book)
            .Where(c => c.UserId == userId && c.ReturnDate == null)
            .Select(c => MapToDto(c))
            .ToListAsync();
    }

    public async Task<List<CheckoutDto>> GetReaderHistory(int userId)
    {
        return await _db.Checkouts
            .Include(c => c.Book)
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CheckoutDate)
            .Select(c => MapToDto(c))
            .ToListAsync();
    }

    public async Task<CheckoutDto> BorrowBook(int readerUserId, int bookId)
    {
        var user = await _db.Users.FindAsync(readerUserId) ?? throw new KeyNotFoundException("Пользователь не найден");
        var book = await _db.Books.FindAsync(bookId) ?? throw new KeyNotFoundException("Книга не найдена");

        if (book.LibraryId != user.LibraryId)
            throw new InvalidOperationException("Вы не можете брать книги из другой библиотеки");

        if (book.AvailableCopies <= 0)
            throw new InvalidOperationException("Нет доступных экземпляров");

        var checkout = new Checkout
        {
            UserId = readerUserId,
            BookId = bookId,
            CheckoutDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14)
        };

        book.AvailableCopies--;

        _db.Checkouts.Add(checkout);
        await _db.SaveChangesAsync();

        return await GetCheckoutDto(checkout.Id);
    }

    public async Task<CheckoutDto> ReturnBook(int librarianLibraryId, int checkoutId)
    {
        var checkout = await _db.Checkouts
            .Include(c => c.Book)
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == checkoutId && c.Book.LibraryId == librarianLibraryId);

        if (checkout == null) throw new KeyNotFoundException("Выдача не найдена");
        return await ProcessReturn(checkout);
    }

    public async Task<CheckoutDto> ReturnBookReader(int userId, int checkoutId)
    {
        var checkout = await _db.Checkouts
            .Include(c => c.Book)
            .FirstOrDefaultAsync(c => c.Id == checkoutId && c.UserId == userId);

        if (checkout == null) throw new KeyNotFoundException("Выдача не найдена");
        return await ProcessReturn(checkout);
    }

    public async Task<CheckoutDto> IssueByLibrarian(int librarianLibraryId, CreateCheckoutDto dto)
    {
        var user = await _db.Users.FindAsync(dto.UserId) ?? throw new KeyNotFoundException("Читатель не найден");
        var book = await _db.Books.FindAsync(dto.BookId) ?? throw new KeyNotFoundException("Книга не найдена");

        if (book.LibraryId != librarianLibraryId || user.LibraryId != librarianLibraryId)
            throw new InvalidOperationException("Операция ограничена вашей библиотекой");

        if (book.AvailableCopies <= 0)
            throw new InvalidOperationException("Нет доступных экземпляров");

        var checkout = new Checkout
        {
            UserId = dto.UserId,
            BookId = dto.BookId,
            CheckoutDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(dto.DurationDays > 0 ? dto.DurationDays : 14)
        };

        book.AvailableCopies--;

        _db.Checkouts.Add(checkout);
        await _db.SaveChangesAsync();

        return await GetCheckoutDto(checkout.Id);
    }

    private async Task<CheckoutDto> ProcessReturn(Checkout checkout)
    {
        if (checkout.ReturnDate != null)
            throw new InvalidOperationException("Книга уже возвращена");

        checkout.ReturnDate = DateTime.UtcNow;
        var book = checkout.Book;
        book.AvailableCopies++;

        if (checkout.ReturnDate > checkout.DueDate)
        {
            var overdueDays = (checkout.ReturnDate.Value - checkout.DueDate).Days;
            checkout.FineAmount = overdueDays * 0.5m;
        }

        await _db.SaveChangesAsync();
        return await GetCheckoutDto(checkout.Id);
    }

    private async Task<CheckoutDto> GetCheckoutDto(int checkoutId)
    {
        var checkout = await _db.Checkouts
            .Include(c => c.Book)
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == checkoutId);

        return MapToDto(checkout!);
    }

    private static CheckoutDto MapToDto(Checkout c) => new()
    {
        Id = c.Id,
        UserId = c.UserId,
        UserName = c.User?.FullName ?? "Unknown",
        BookId = c.BookId,
        BookTitle = c.Book?.Title ?? "Unknown",
        CheckoutDate = c.CheckoutDate,
        DueDate = c.DueDate,
        ReturnDate = c.ReturnDate,
        FineAmount = c.FineAmount
    };
}