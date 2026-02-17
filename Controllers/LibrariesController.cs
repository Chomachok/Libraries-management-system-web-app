using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibrariesWebApp.Data;
using LibrariesWebApp.Models;

namespace LibrariesWebApp.Controllers;

/// <summary>
/// Контроллер для управления библиотеками (CRUD операции).
/// Обеспечивает отображение списка, создание, редактирование и удаление записей о библиотеках.
/// </summary>
public class LibrariesController(AppDbContext context) : Controller
{
    /// <summary>
    /// Отображает страницу со списком всех библиотек.
    /// </summary>
    /// <returns>Представление Index, содержащее коллекцию объектов Library.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Library>>> Index()
    {
        return View(await context.Libraries.ToListAsync());
    }

    /// <summary>
    /// Отображает форму для создания новой библиотеки.
    /// </summary>
    /// <returns>Представление Create с пустой формой.</returns>
    [HttpGet]
    public IActionResult Create()
    {
        return View("Create");
    }

    /// <summary>
    /// Обрабатывает отправку формы создания новой библиотеки.
    /// </summary>
    /// <param name="library">Объект библиотеки, переданный из формы.</param>
    /// <returns>
    /// При успешном добавлении выполняет перенаправление на действие Index.
    /// При ошибках валидации возвращает представление Index (с текущим списком библиотек и сообщениями об ошибках).
    /// </returns>
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

    /// <summary>
    /// Отображает страницу подтверждения удаления библиотеки.
    /// </summary>
    /// <param name="id">Идентификатор библиотеки.</param>
    /// <returns>
    /// Возвращает представление Delete с данными библиотеки, если она найдена.
    /// Если id не указан или библиотека не существует, возвращает HTTP 404 Not Found.
    /// </returns>
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

    /// <summary>
    /// Подтверждает удаление библиотеки.
    /// </summary>
    /// <param name="id">Идентификатор библиотеки.</param>
    /// <returns>Перенаправляет на действие Index после удаления.</returns>
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

    /// <summary>
    /// Отображает форму для редактирования существующей библиотеки.
    /// </summary>
    /// <param name="id">Идентификатор библиотеки.</param>
    /// <returns>
    /// Представление Edit с данными библиотеки для редактирования.
    /// Если id не указан или библиотека не найдена, возвращает HTTP 404 Not Found.
    /// </returns>
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

    /// <summary>
    /// Обрабатывает отправку формы редактирования библиотеки.
    /// </summary>
    /// <param name="id">Идентификатор редактируемой библиотеки (должен совпадать с LibraryId объекта).</param>
    /// <param name="library">Объект с обновлёнными данными.</param>
    /// <returns>
    /// При успешном обновлении перенаправляет на Index.
    /// При ошибках валидации возвращает представление Edit с текущим объектом и сообщениями об ошибках.
    /// При конфликте параллельного обновления (DbUpdateConcurrencyException) повторно проверяет существование записи.
    /// </returns>
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

                throw; // Если запись существует, но конфликт не устранён, пробрасываем исключение дальше
            }
        }

        return View(library);
    }

    /// <summary>
    /// Проверяет, существует ли библиотека с указанным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор библиотеки.</param>
    /// <returns>True, если запись существует; иначе False.</returns>
    private bool LibraryExists(int id)
    {
        return context.Libraries.Any(e => e.LibraryId == id);
    }
}