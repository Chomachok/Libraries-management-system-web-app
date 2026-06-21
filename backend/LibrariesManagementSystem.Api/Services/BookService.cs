using LibrariesManagementSystem.Api.Data;
using LibrariesManagementSystem.Api.DTOs.Book;
using LibrariesManagementSystem.Api.Models;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrariesManagementSystem.Api.Services;

/// <summary>
/// Реализация сервиса для управления книгами.
/// Обеспечивает получение, создание, обновление и удаление книг в контексте библиотеки.
/// </summary>
public class BookService(AppDbContext db) : IBookService
{
    
    /// <summary>
    /// Получить список книг с возможностью фильтрации по библиотеке и поиска по названию или автору.
    /// </summary>
    /// <param name="libraryId">ID библиотеки для фильтрации (null - все библиотеки).</param>
    /// <param name="search">Строка поиска (null или пустая строка - без поиска).</param>
    /// <returns>Список книг в виде <see cref="BookDto"/>.</returns>
    public async Task<List<BookDto>> GetBooks(int? libraryId, string? search)
    {
        var query = db.Books.Include(b => b.Library).AsQueryable();

        if (libraryId.HasValue)
            query = query.Where(b => b.LibraryId == libraryId.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower();
            query = query.Where(b => b.Title.ToLower().Contains(s) || b.Author.ToLower().Contains(s));
        }

        return await query.Select(b => MapToDto(b)).ToListAsync();
    }

    /// <summary>
    /// Получить книгу по идентификатору.
    /// </summary>
    /// <param name="id">ID книги.</param>
    /// <returns>Данные книги в виде <see cref="BookDto"/>.</returns>
    /// <exception cref="KeyNotFoundException">Книга с указанным ID не найдена.</exception>
    public async Task<BookDto> GetById(int id)
    {
        var book = await db.Books.Include(b => b.Library).FirstOrDefaultAsync(b => b.Id == id);
        if (book == null) throw new KeyNotFoundException("Книга не найдена");
        return MapToDto(book);
    }

    /// <summary>
    /// Создать новую книгу в библиотеке библиотекаря.
    /// При создании количество доступных копий устанавливается равным общему количеству.
    /// </summary>
    /// <param name="librarianLibraryId">ID библиотеки, куда добавляется книга.</param>
    /// <param name="dto">Данные для создания книги.</param>
    /// <returns>Созданная книга в виде <see cref="BookDto"/>.</returns>
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
            LibraryId = librarianLibraryId,
            CoverImageUrl = dto.CoverImageUrl
        };

        db.Books.Add(book);
        await db.SaveChangesAsync();

        return await GetById(book.Id);
    }
    
    /// <summary>
    /// Обновить данные существующей книги. Доступные копии пересчитываются с учётом изменения общего количества.
    /// </summary>
    /// <param name="librarianLibraryId">ID библиотеки, которой принадлежит книга.</param>
    /// <param name="bookId">ID обновляемой книги.</param>
    /// <param name="dto">Новые данные книги.</param>
    /// <returns>Обновлённая книга в виде <see cref="BookDto"/>.</returns>
    /// <exception cref="KeyNotFoundException">Книга не найдена в указанной библиотеке.</exception>
    public async Task<BookDto> Update(int librarianLibraryId, int bookId, UpdateBookDto dto)
    {
        var book = await db.Books.FirstOrDefaultAsync(b => b.Id == bookId && b.LibraryId == librarianLibraryId);
        if (book == null) throw new KeyNotFoundException("Книга не найдена");

        var diff = dto.TotalCopies - book.TotalCopies;
        book.Title = dto.Title;
        book.Author = dto.Author;
        book.ISBN = dto.ISBN;
        book.Genre = dto.Genre;
        book.Year = dto.Year;
        book.TotalCopies = dto.TotalCopies;
        book.AvailableCopies = Math.Max(0, book.AvailableCopies + diff);
        book.CoverImageUrl = dto.CoverImageUrl;

        await db.SaveChangesAsync();
        return await GetById(book.Id);
    }

    /// <summary>
    /// Удалить книгу. Удаление возможно только при отсутствии активных выдач.
    /// </summary>
    /// <param name="librarianLibraryId">ID библиотеки, из которой удаляется книга.</param>
    /// <param name="bookId">ID удаляемой книги.</param>
    /// <exception cref="KeyNotFoundException">Книга не найдена.</exception>
    /// <exception cref="InvalidOperationException">Книга имеет активные выдачи и не может быть удалена.</exception>
    public async Task Delete(int librarianLibraryId, int bookId)
    {
        var book = await db.Books
            .Include(b => b.Checkouts)
            .FirstOrDefaultAsync(b => b.Id == bookId && b.LibraryId == librarianLibraryId);

        if (book == null) throw new KeyNotFoundException("Книга не найдена");
        if (book.Checkouts.Any(c => c.ReturnDate == null))
            throw new InvalidOperationException("Невозможно удалить книгу с активными выдачами");

        db.Books.Remove(book);
        await db.SaveChangesAsync();
    }

    /// <summary>
    /// Преобразует сущность <see cref="Book"/> в DTO <see cref="BookDto"/> для передачи клиенту.
    /// </summary>
    /// <param name="b">Сущность книги.</param>
    /// <returns>Объект <see cref="BookDto"/>.</returns>
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
        LibraryName = b.Library?.Name ?? "",
        CoverImageUrl = b.CoverImageUrl,
    };
}
