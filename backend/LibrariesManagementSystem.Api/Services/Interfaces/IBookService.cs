using LibrariesManagementSystem.Api.DTOs.Book;

namespace LibrariesManagementSystem.Api.Services.Interfaces;

public interface IBookService
{
    Task<List<BookDto>> GetBooks(int? libraryId, string? search);
    Task<BookDto> GetById(int id);
    Task<BookDto> Create(int librarianLibraryId, CreateBookDto dto);
    Task<BookDto> Update(int librarianLibraryId, int bookId, UpdateBookDto dto);
    Task Delete(int librarianLibraryId, int bookId);
}