using LibrariesManagementSystem.Api.DTOs.Library;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesManagementSystem.Api.Controllers;

/// <summary>
/// Контроллер для получения списка библиотек.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LibrariesController : ControllerBase
{
    private readonly ILibraryService _libraryService;

    public LibrariesController(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    /// <summary>
    /// Получить список всех библиотек системы.
    /// </summary>
    /// <returns>Список объектов <see cref="LibraryDto"/>.</returns>
    /// <response code="200">Список библиотек успешно получен.</response>
    [HttpGet]
    public async Task<ActionResult<List<LibraryDto>>> GetAll()
    {
        var libraries = await _libraryService.GetAll();
        return Ok(libraries);
    }
}