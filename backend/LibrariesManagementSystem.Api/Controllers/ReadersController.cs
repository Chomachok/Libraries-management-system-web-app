using System.Security.Claims;
using LibrariesManagementSystem.Api.DTOs.User;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReadersController(IUserService userService) : ControllerBase
{
    [Authorize(Roles = "Librarian")]
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAll()
    {
        var libId = GetLibraryId();
        var readers = await userService.GetReaders(libId);
        return Ok(readers);
    }

    [Authorize(Roles = "Librarian")]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var libId = GetLibraryId();
        var user = await userService.GetById(id);
        if (user.LibraryId != libId) return Forbid();
        return Ok(user);
    }

    [Authorize(Roles = "Librarian")]
    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var libId = GetLibraryId();
        var reader = await userService.CreateReader(libId, dto);
        return CreatedAtAction(nameof(GetById), new { id = reader.Id }, reader);
    }

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

    [Authorize(Roles = "Librarian")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var libId = GetLibraryId();
        await userService.Delete(libId, id);
        return NoContent();
    }

    // Профиль текущего пользователя
    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<UserDto>> GetProfile()
    {
        int userId = GetUserId();
        var user = await userService.GetById(userId);
        return Ok(user);
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<ActionResult<UserDto>> UpdateProfile([FromBody] UpdateUserDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userId = GetUserId();
        var updated = await userService.UpdateOwnProfile(userId, dto);
        return Ok(updated);
    }

    private int GetLibraryId() => int.Parse(User.FindFirst("LibraryId")!.Value);
    private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
}