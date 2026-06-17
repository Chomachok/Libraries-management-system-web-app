using System.Security.Claims;
using LibrariesManagementSystem.Api.DTOs.Checkout;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesManagementSystem.Api.Controllers;

/// <summary>
/// Контроллер для управления выдачей и возвратом книг.
/// Разделяет зоны ответственности читателя и библиотекаря.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CheckoutsController(ICheckoutService checkoutService) : ControllerBase
{
    /// <summary>
    /// Получить список активных выдач текущего читателя.
    /// </summary>
    /// <returns>Список <see cref="CheckoutDto"/> с книгами, которые находятся у читателя.</returns>
    /// <response code="200">Список активных выдач успешно получен.</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    /// <response code="403">Недостаточно прав (требуется роль Reader).</response>
    [Authorize(Roles = "Reader")]
    [HttpGet("my-active")]
    public async Task<ActionResult<List<CheckoutDto>>> GetMyActive()
    {
        var userId = GetUserId();
        var checkouts = await checkoutService.GetReaderActiveCheckouts(userId);
        return Ok(checkouts);
    }

    /// <summary>
    /// Получить историю всех выдач текущего читателя.
    /// </summary>
    /// <returns>Список <see cref="CheckoutDto"/> со всеми выдачами (включая возвращённые).</returns>
    /// <response code="200">История выдач успешно получена.</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    /// <response code="403">Недостаточно прав (требуется роль Reader).</response>
    [Authorize(Roles = "Reader")]
    [HttpGet("my-history")]
    public async Task<ActionResult<List<CheckoutDto>>> GetMyHistory()
    {
        var userId = GetUserId();
        var history = await checkoutService.GetReaderHistory(userId);
        return Ok(history);
    }

    /// <summary>
    /// Читатель берёт книгу.
    /// </summary>
    /// <param name="bookId">Идентификатор книги, которую нужно взять.</param>
    /// <returns>Созданная запись выдачи <see cref="CheckoutDto"/>.</returns>
    /// <response code="200">Книга успешно взята.</response>
    /// <response code="400">Книга недоступна или читатель превысил лимит.</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    /// <response code="403">Недостаточно прав (требуется роль Reader).</response>
    [Authorize(Roles = "Reader")]
    [HttpPost("borrow/{bookId}")]
    public async Task<ActionResult<CheckoutDto>> Borrow(int bookId)
    {
        var userId = GetUserId();
        var checkout = await checkoutService.BorrowBook(userId, bookId);
        return Ok(checkout);
    }

    /// <summary>
    /// Читатель возвращает книгу.
    /// </summary>
    /// <param name="checkoutId">Идентификатор записи выдачи, по которой производится возврат.</param>
    /// <returns>Обновлённая запись выдачи с датой возврата.</returns>
    /// <response code="200">Книга успешно возвращена.</response>
    /// <response code="400">Некорректный запрос (например, книга уже возвращена).</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    /// <response code="403">Недостаточно прав (требуется роль Reader).</response>
    /// <response code="404">Запись выдачи не найдена.</response>
    [Authorize(Roles = "Reader")]
    [HttpPost("return/{checkoutId}")]
    public async Task<ActionResult<CheckoutDto>> Return(int checkoutId)
    {
        var userId = GetUserId();
        var checkout = await checkoutService.ReturnBookReader(userId, checkoutId);
        return Ok(checkout);
    }

    /// <summary>
    /// Библиотекарь получает все выдачи в своей библиотеке.
    /// </summary>
    /// <returns>Список <see cref="CheckoutDto"/> всех выдач (активных и завершённых) в библиотеке.</returns>
    /// <response code="200">Список выдач успешно получен.</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    /// <response code="403">Недостаточно прав (требуется роль Librarian).</response>
    [Authorize(Roles = "Librarian")]
    [HttpGet("library")]
    public async Task<ActionResult<List<CheckoutDto>>> GetLibraryCheckouts()
    {
        var libId = GetLibraryId();
        var checkouts = await checkoutService.GetLibraryCheckouts(libId);
        return Ok(checkouts);
    }

    /// <summary>
    /// Библиотекарь выдаёт книгу читателю.
    /// </summary>
    /// <param name="dto">Данные для создания выдачи: идентификатор читателя и идентификатор книги.</param>
    /// <returns>Созданная запись выдачи <see cref="CheckoutDto"/>.</returns>
    /// <response code="200">Книга успешно выдана.</response>
    /// <response code="400">Некорректные данные или нарушение бизнес-правил.</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    /// <response code="403">Недостаточно прав (требуется роль Librarian).</response>
    [Authorize(Roles = "Librarian")]
    [HttpPost("issue")]
    public async Task<ActionResult<CheckoutDto>> Issue([FromBody] CreateCheckoutDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var libId = GetLibraryId();
        var checkout = await checkoutService.IssueByLibrarian(libId, dto);
        return Ok(checkout);
    }

    /// <summary>
    /// Библиотекарь принимает возврат книги от читателя.
    /// </summary>
    /// <param name="checkoutId">Идентификатор записи выдачи, по которой производится возврат.</param>
    /// <returns>Обновлённая запись выдачи с датой возврата.</returns>
    /// <response code="200">Книга успешно принята (возвращена).</response>
    /// <response code="400">Некорректный запрос (книга уже возвращена и т.п.).</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    /// <response code="403">Недостаточно прав (требуется роль Librarian).</response>
    /// <response code="404">Запись выдачи не найдена.</response>
    [Authorize(Roles = "Librarian")]
    [HttpPost("return-by-librarian/{checkoutId}")]
    public async Task<ActionResult<CheckoutDto>> ReturnByLibrarian(int checkoutId)
    {
        var libId = GetLibraryId();
        var checkout = await checkoutService.ReturnBook(libId, checkoutId);
        return Ok(checkout);
    }

    /// <summary>
    /// Получить идентификатор текущего пользователя из claims.
    /// </summary>
    private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    /// <summary>
    /// Получить идентификатор библиотеки текущего пользователя из claims.
    /// </summary>
    private int GetLibraryId() => int.Parse(User.FindFirst("LibraryId")!.Value);
}
