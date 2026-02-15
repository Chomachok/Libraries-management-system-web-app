using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibrariesWebApp.Data;
using LibrariesWebApp.Models;

namespace LibrariesWebApp.Controllers;

public class LibraryController(AppDbContext context) : Controller
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Library>>> GetLibraries()
    {
        return await context.Libraries.ToListAsync();
    }

    [HttpGet("id")]
    public async Task<ActionResult<Library>> GetLibrary(int id)
    {
        var library = context.Libraries.FindAsync(id);

        if (library != null)
        {
            return await library;
        }

        throw new KeyNotFoundException("Library not found");
    }
}