using System.Security.Claims;
using LibrariesManagementSystem.Api.DTOs.User;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesManagementSystem.Api.Controllers;

/// <summary>
/// Контроллер для управления читателями (учётными записями пользователей с ролью Reader).
/// Библиотекари могут управлять читателями своей библиотеки, а текущий пользователь - своим профилем.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReadersController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Получить список всех читателей библиотеки текущего библиотекаря.
    /// </summary>
    /// <returns>Список читателей <see cref="UserDto"/>.</returns>
    /// <response code="200">Список успешно получен.</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    /// <response code="403">Недостаточно прав (требуется роль Librarian).</response>
    [Authorize(Roles = "Librarian")]
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAll()
    {
        var libId = GetLibraryId();
        var readers = await userService.GetReaders(libId);
        return Ok(readers);
    }

    /// <summary>
    /// Получить читателя по идентификатору. Доступен только библиотекарю той же библиотеки.
    /// </summary>
    /// <param name="id">ID читателя.</param>
    /// <returns>Данные читателя <see cref="UserDto"/>.</returns>
    /// <response code="200">Читатель найден и возвращён.</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    /// <response code="403">Доступ запрещён (роль или принадлежность к другой библиотеке).</response>
    /// <response code="404">Читатель не найден.</response>
    [Authorize(Roles = "Librarian")]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var libId = GetLibraryId();
        var user = await userService.GetById(id);
        if (user.LibraryId != libId) return Forbid();
        return Ok(user);
    }

    /// <summary>
    /// Библиотекарь создаёт нового читателя в своей библиотеке.
    /// </summary>
    /// <param name="dto">Данные для создания читателя.</param>
    /// <returns>Созданный читатель <see cref="UserDto"/> с кодом 201.</returns>
    /// <response code="201">Читатель успешно создан.</response>
    /// <response code="400">Некорректные данные.</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    /// <response code="403">Недостаточно прав.</response>
    [Authorize(Roles = "Librarian")]
    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var libId = GetLibraryId();
        var reader = await userService.CreateReader(libId, dto);
        return CreatedAtAction(nameof(GetById), new { id = reader.Id }, reader);
    }

    /// <summary>
    /// Библиотекарь обновляет данные читателя своей библиотеки.
    /// </summary>
    /// <param name="id">ID читателя.</param>
    /// <param name="dto">Новые данные читателя.</param>
    /// <returns>Обновлённый читатель <see cref="UserDto"/>.</returns>
    /// <response code="200">Данные успешно обновлены.</response>
    /// <response code="400">Некорректные данные.</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    /// <response code="403">Доступ запрещён (не библиотекарь или читатель из другой библиотеки).</response>
    /// <response code="404">Читатель не найден.</response>
    [Authorize(Roles = "Librarian")]
    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var libId = GetLibraryId();
        var existing = await userService.GetById(id);
        if (existing.LibraryId != libId) return Forbid();
        var updated = await userService.Update(id, dto);
        return Ok(updated);
    }

    /// <summary>
    /// Библиотекарь удаляет читателя из своей библиотеки.
    /// </summary>
    /// <param name="id">ID читателя.</param>
    /// <returns>204 No Content при успешном удалении.</returns>
    /// <response code="204">Читатель удалён.</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    /// <response code="403">Недостаточно прав.</response>
    /// <response code="404">Читатель не найден.</response>
    [Authorize(Roles = "Librarian")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var libId = GetLibraryId();
        await userService.Delete(libId, id);
        return NoContent();
    }

    /// <summary>
    /// Получить профиль текущего аутентифицированного пользователя (любая роль).
    /// </summary>
    /// <returns>Профиль пользователя <see cref="UserDto"/>.</returns>
    /// <response code="200">Профиль успешно получен.</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<UserDto>> GetProfile()
    {
        int userId = GetUserId();
        var user = await userService.GetById(userId);
        return Ok(user);
    }

    /// <summary>
    /// Обновить собственный профиль текущего пользователя.
    /// </summary>
    /// <param name="dto">Новые данные профиля.</param>
    /// <returns>Обновлённый профиль <see cref="UserDto"/>.</returns>
    /// <response code="200">Профиль успешно обновлён.</response>
    /// <response code="400">Некорректные данные.</response>
    /// <response code="401">Пользователь не аутентифицирован.</response>
    [Authorize]
    [HttpPut("profile")]
    public async Task<ActionResult<UserDto>> UpdateProfile([FromBody] UpdateUserDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userId = GetUserId();
        var updated = await userService.UpdateOwnProfile(userId, dto);
        return Ok(updated);
    }

    /// <summary>
    /// Получить идентификатор библиотеки текущего пользователя из claims.
    /// </summary>
    private int GetLibraryId() => int.Parse(User.FindFirst("LibraryId")!.Value);
    
    /// <summary>
    /// Получить идентификатор текущего пользователя из claims.
    /// </summary>
    private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
}