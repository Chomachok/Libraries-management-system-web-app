using LibrariesManagementSystem.Api.DTOs.Book;

namespace LibrariesManagementSystem.Api.Services.Interfaces;

/// <summary>
/// Сервис для управления книгами в библиотеках.
/// </summary>
public interface IBookService
{
    /// <summary>
    /// Получить список книг с возможностью фильтрации по библиотеке и поиска по названию/автору.
    /// </summary>
    /// <param name="libraryId">ID библиотеки для фильтрации (null — все библиотеки).</param>
    /// <param name="search">Строка поиска (null - без поиска).</param>
    /// <returns>Список книг в виде <see cref="BookDto"/>.</returns>
    Task<List<BookDto>> GetBooks(int? libraryId, string? search);
    
    /// <summary>
    /// Получить книгу по идентификатору.
    /// </summary>
    /// <param name="id">ID книги.</param>
    /// <returns>Данные книги в виде <see cref="BookDto"/>.</returns>
    Task<BookDto> GetById(int id);
    
    /// <summary>
    /// Создать новую книгу в библиотеке библиотекаря.
    /// </summary>
    /// <param name="librarianLibraryId">ID библиотеки, в которую добавляется книга.</param>
    /// <param name="dto">Данные для создания книги.</param>
    /// <returns>Созданная книга в виде <see cref="BookDto"/>.</returns>
    Task<BookDto> Create(int librarianLibraryId, CreateBookDto dto);
    
    /// <summary>
    /// Обновить данные книги.
    /// </summary>
    /// <param name="librarianLibraryId">ID библиотеки, к которой принадлежит книга.</param>
    /// <param name="bookId">ID обновляемой книги.</param>
    /// <param name="dto">Новые данные книги.</param>
    /// <returns>Обновлённая книга в виде <see cref="BookDto"/>.</returns>
    Task<BookDto> Update(int librarianLibraryId, int bookId, UpdateBookDto dto);
    
    /// <summary>
    /// Удалить книгу.
    /// </summary>
    /// <param name="librarianLibraryId">ID библиотеки, из которой удаляется книга.</param>
    /// <param name="bookId">ID удаляемой книги.</param>
    Task Delete(int librarianLibraryId, int bookId);
}
