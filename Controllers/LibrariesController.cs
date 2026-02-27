using LibrariesWebApp.Models;
using LibrariesWebApp.Controllers.Base;
using LibrariesWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesWebApp.Controllers;

/// <inheritdoc />
public class LibrariesController(ICrudService<Library, int> service) : CrudController<Library, int>(service)
{
    private readonly ICrudService<Library, int> _libraryService = service;

    /// <summary>
    /// Отображает подробную информацию о библиотеке, включая список книг.
    /// </summary>
    /// <param name="id">Идентификатор библиотеки.</param>
    /// <returns>Представление с данными библиотеки.</returns>
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var library = await _libraryService.GetByIdAsync(id);
        return View(library);
    }
}