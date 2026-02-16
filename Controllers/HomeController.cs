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
