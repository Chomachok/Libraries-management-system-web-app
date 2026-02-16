using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibrariesWebApp.Data;
using LibrariesWebApp.Models;

namespace LibrariesWebApp.Controllers;

/// <summary>
/// Index - возвращение представления главной страницы со списком библиотек
/// Create (GET) - возвращение представления страницы создания страницы
/// Create (POST) - добавление в бд новой библиотеки
/// Delete (GET) - проверка есть ли такая библиотека в бд и вывод её представления
/// DeleteConfirmed (POST) - удаление библиотеки из бд
/// </summary>

public class LibrariesController(AppDbContext context) : Controller
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Library>>> Index()
    {
        return await context.Libraries.ToListAsync();
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View("Create");
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(Library library)
    {
        if (ModelState.IsValid)
        {
            context.Libraries.Add(library);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        return View("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();
        var library = await context.Libraries.FindAsync(id);

        if (library == null)
            return NotFound();
        
        return View(library);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var library = await context.Libraries.FindAsync(id);

        if (library != null)
        {
            context.Libraries.Remove(library);
            await context.SaveChangesAsync();
        }
        
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();
        var library = await context.Libraries.FindAsync(id);

        if (library == null)
            return NotFound();
        
        return View(library);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Library library)
    {
        if (id != library.LibraryId)
            return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                context.Update(library);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibraryExists(library.LibraryId))
                    return NotFound();
                
                throw;
            }
        }
        
        return View(library);
    }
    
    private bool LibraryExists(int id) => context.Libraries.Any(e => e.LibraryId == id);
}