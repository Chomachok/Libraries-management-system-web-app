using System.Security.Claims;
using LibrariesManagementSystem.Api.DTOs.Checkout;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckoutsController(ICheckoutService checkoutService) : ControllerBase
{
    // Читатель: активные книги
    [Authorize(Roles = "Reader")]
    [HttpGet("my-active")]
    public async Task<ActionResult<List<CheckoutDto>>> GetMyActive()
    {
        var userId = GetUserId();
        var checkouts = await checkoutService.GetReaderActiveCheckouts(userId);
        return Ok(checkouts);
    }

    // Читатель: история
    [Authorize(Roles = "Reader")]
    [HttpGet("my-history")]
    public async Task<ActionResult<List<CheckoutDto>>> GetMyHistory()
    {
        var userId = GetUserId();
        var history = await checkoutService.GetReaderHistory(userId);
        return Ok(history);
    }

    // Читатель: взять книгу
    [Authorize(Roles = "Reader")]
    [HttpPost("borrow/{bookId}")]
    public async Task<ActionResult<CheckoutDto>> Borrow(int bookId)
    {
        var userId = GetUserId();
        var checkout = await checkoutService.BorrowBook(userId, bookId);
        return Ok(checkout);
    }

    // Читатель: вернуть книгу
    [Authorize(Roles = "Reader")]
    [HttpPost("return/{checkoutId}")]
    public async Task<ActionResult<CheckoutDto>> Return(int checkoutId)
    {
        var userId = GetUserId();
        var checkout = await checkoutService.ReturnBookReader(userId, checkoutId);
        return Ok(checkout);
    }

    // Библиотекарь: все выдачи библиотеки
    [Authorize(Roles = "Librarian")]
    [HttpGet("library")]
    public async Task<ActionResult<List<CheckoutDto>>> GetLibraryCheckouts()
    {
        var libId = GetLibraryId();
        var checkouts = await checkoutService.GetLibraryCheckouts(libId);
        return Ok(checkouts);
    }

    // Библиотекарь: выдать книгу читателю
    [Authorize(Roles = "Librarian")]
    [HttpPost("issue")]
    public async Task<ActionResult<CheckoutDto>> Issue([FromBody] CreateCheckoutDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var libId = GetLibraryId();
        var checkout = await checkoutService.IssueByLibrarian(libId, dto);
        return Ok(checkout);
    }

    // Библиотекарь: принять возврат
    [Authorize(Roles = "Librarian")]
    [HttpPost("return-by-librarian/{checkoutId}")]
    public async Task<ActionResult<CheckoutDto>> ReturnByLibrarian(int checkoutId)
    {
        var libId = GetLibraryId();
        var checkout = await checkoutService.ReturnBook(libId, checkoutId);
        return Ok(checkout);
    }

    private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    private int GetLibraryId() => int.Parse(User.FindFirst("LibraryId")!.Value);
}