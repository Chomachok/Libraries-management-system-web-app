using LibrariesManagementSystem.Api.DTOs.Book;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(IBookService bookService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<BookDto>>> GetAll([FromQuery] int? libraryId, [FromQuery] string? search)
    {
        var books = await bookService.GetBooks(libraryId, search);
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetById(int id)
    {
        var book = await bookService.GetById(id);
        return Ok(book);
    }

    [Authorize(Roles = "Librarian")]
    [HttpPost]
    public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        int libId = GetLibraryId();
        var book = await bookService.Create(libId, dto);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    [Authorize(Roles = "Librarian")]
    [HttpPut("{id}")]
    public async Task<ActionResult<BookDto>> Update(int id, [FromBody] UpdateBookDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        int libId = GetLibraryId();
        var book = await bookService.Update(libId, id, dto);
        return Ok(book);
    }

    [Authorize(Roles = "Librarian")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        int libId = GetLibraryId();
        await bookService.Delete(libId, id);
        return NoContent();
    }

    private int GetLibraryId()
    {
        return int.Parse(User.FindFirst("LibraryId")!.Value);
    }
}