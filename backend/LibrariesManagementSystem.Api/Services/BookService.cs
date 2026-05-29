using LibrariesManagementSystem.Api.Data;
using LibrariesManagementSystem.Api.DTOs.Book;
using LibrariesManagementSystem.Api.Models;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrariesManagementSystem.Api.Services;

public class BookService : IBookService
{
    private readonly AppDbContext _db;
    public BookService(AppDbContext db) => _db = db;

    public async Task<List<BookDto>> GetBooks(int? libraryId, string? search)
    {
        var query = _db.Books.Include(b => b.Library).AsQueryable();

        if (libraryId.HasValue)
            query = query.Where(b => b.LibraryId == libraryId.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower();
            query = query.Where(b => b.Title.ToLower().Contains(s) || b.Author.ToLower().Contains(s));
        }

        return await query.Select(b => MapToDto(b)).ToListAsync();
    }

    public async Task<BookDto> GetById(int id)
    {
        var book = await _db.Books.Include(b => b.Library).FirstOrDefaultAsync(b => b.Id == id);
        if (book == null) throw new KeyNotFoundException("Книга не найдена");
        return MapToDto(book);
    }

    public async Task<BookDto> Create(int librarianLibraryId, CreateBookDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            ISBN = dto.ISBN,
            Genre = dto.Genre,
            Year = dto.Year,
            TotalCopies = dto.TotalCopies,
            AvailableCopies = dto.TotalCopies,
            LibraryId = librarianLibraryId
        };

        _db.Books.Add(book);
        await _db.SaveChangesAsync();

        return await GetById(book.Id);
    }

    public async Task<BookDto> Update(int librarianLibraryId, int bookId, UpdateBookDto dto)
    {
        var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == bookId && b.LibraryId == librarianLibraryId);
        if (book == null) throw new KeyNotFoundException("Книга не найдена");

        int diff = dto.TotalCopies - book.TotalCopies;
        book.Title = dto.Title;
        book.Author = dto.Author;
        book.ISBN = dto.ISBN;
        book.Genre = dto.Genre;
        book.Year = dto.Year;
        book.TotalCopies = dto.TotalCopies;
        book.AvailableCopies = Math.Max(0, book.AvailableCopies + diff);

        await _db.SaveChangesAsync();
        return await GetById(book.Id);
    }

    public async Task Delete(int librarianLibraryId, int bookId)
    {
        var book = await _db.Books
            .Include(b => b.Checkouts)
            .FirstOrDefaultAsync(b => b.Id == bookId && b.LibraryId == librarianLibraryId);

        if (book == null) throw new KeyNotFoundException("Книга не найдена");
        if (book.Checkouts.Any(c => c.ReturnDate == null))
            throw new InvalidOperationException("Невозможно удалить книгу с активными выдачами");

        _db.Books.Remove(book);
        await _db.SaveChangesAsync();
    }

    private static BookDto MapToDto(Book b) => new()
    {
        Id = b.Id,
        Title = b.Title,
        Author = b.Author,
        ISBN = b.ISBN,
        Genre = b.Genre,
        Year = b.Year,
        TotalCopies = b.TotalCopies,
        AvailableCopies = b.AvailableCopies,
        LibraryId = b.LibraryId,
        LibraryName = b.Library?.Name ?? ""
    };
}