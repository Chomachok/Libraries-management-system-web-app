using LibrariesManagementSystem.Api.DTOs.Library;
using LibrariesManagementSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrariesManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibrariesController : ControllerBase
{
    private readonly ILibraryService _libraryService;

    public LibrariesController(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<LibraryDto>>> GetAll()
    {
        var libraries = await _libraryService.GetAll();
        return Ok(libraries);
    }
}