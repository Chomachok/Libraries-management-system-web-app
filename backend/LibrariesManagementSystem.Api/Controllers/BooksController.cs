using LibrariesManagementSystem.Api.DTOs.Book;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesManagementSystem.Api.Controllers;

/// <summary>
/// Контроллер для управления книгами в библиотеках.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BooksController(IBookService bookService) : ControllerBase
{
    /// <summary>
    /// Получить список книг с возможностью фильтрации по библиотеке и поиска.
    /// </summary>
    /// <param name="libraryId">ID библиотеки для фильтрации.</param>
    /// <param name="search">Строка поиска по названию или автору.</param>
    /// <returns>Список книг в формате <see cref="BookDto"/>.</returns>
    /// <response code="200">Список книг успешно получен.</response>
    [HttpGet]
    public async Task<ActionResult<List<BookDto>>> GetAll([FromQuery] int? libraryId, [FromQuery] string? search)
    {
        var books = await bookService.GetBooks(libraryId, search);
        return Ok(books);
    }

    /// <summary>
    /// Получить книгу по идентификатору.
    /// </summary>
    /// <param name="id">ID книги.</param>
    /// <returns>Данные книги в формате <see cref="BookDto"/>.</returns>
    /// <response code="200">Книга найдена и возвращена.</response>
    /// <response code="404">Книга с указанным ID не найдена.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetById(int id)
    {
        var book = await bookService.GetById(id);
        return Ok(book);
    }

    /// <summary>
    /// Добавить новую книгу. Требуется роль Librarian.
    /// </summary>
    /// <param name="dto">Данные создаваемой книги.</param>
    /// <returns>Созданная книга с кодом 201 и заголовком Location.</returns>
    /// <response code="201">Книга успешно создана.</response>
    /// <response code="400">Некорректные данные.</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    /// <response code="403">Недостаточно прав (требуется роль Librarian).</response>
    [Authorize(Roles = "Librarian")]
    [HttpPost]
    public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        int libId = GetLibraryId();
        var book = await bookService.Create(libId, dto);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    /// <summary>
    /// Обновить существующую книгу. Требуется роль Librarian.
    /// </summary>
    /// <param name="id">ID обновляемой книги.</param>
    /// <param name="dto">Новые данные книги.</param>
    /// <returns>Обновлённая книга.</returns>
    /// <response code="200">Книга успешно обновлена.</response>
    /// <response code="400">Некорректные данные.</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    /// <response code="403">Недостаточно прав.</response>
    /// <response code="404">Книга не найдена.</response>
    [Authorize(Roles = "Librarian")]
    [HttpPut("{id}")]
    public async Task<ActionResult<BookDto>> Update(int id, [FromBody] UpdateBookDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        int libId = GetLibraryId();
        var book = await bookService.Update(libId, id, dto);
        return Ok(book);
    }

    /// <summary>
    /// Удалить книгу. Требуется роль Librarian.
    /// </summary>
    /// <param name="id">ID удаляемой книги.</param>
    /// <returns>204 No Content при успешном удалении.</returns>
    /// <response code="204">Книга удалена.</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    /// <response code="403">Недостаточно прав.</response>
    /// <response code="404">Книга не найдена.</response>
    [Authorize(Roles = "Librarian")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        int libId = GetLibraryId();
        await bookService.Delete(libId, id);
        return NoContent();
    }

    /// <summary>
    /// Извлекает ID библиотеки из claims текущего пользователя.
    /// </summary>
    /// <returns>Идентификатор библиотеки.</returns>
    private int GetLibraryId()
    {
        return int.Parse(User.FindFirst("LibraryId")!.Value);
    }
}
