using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibrariesWebApp.Models;
using LibrariesWebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace LibrariesWebApp.Controllers;

public class HomeController(AppDbContext context) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        Console.WriteLine(string.Join(", ", context.Books.ToList()));
        return await context.Books.ToListAsync();
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Library>>> GetLibraries()
    {
        Console.WriteLine(string.Join(", ", context.Libraries.ToList()));
        return await context.Libraries.ToListAsync();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
