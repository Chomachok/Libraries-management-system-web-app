using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibrariesWebApp.Data;
using LibrariesWebApp.Models;

namespace LibrariesWebApp.Controllers;

/// <summary>
/// Контроллер для управления издателями (CRUD операции).
/// Обеспечивает отображение списка, создание, редактирование и удаление записей об издателях.
/// </summary>
public class PublishersController(AppDbContext context) : Controller
{
    /// <summary>
    /// Отображает страницу со списком всех издателей.
    /// </summary>
    /// <returns>Представление Index, содержащее коллекцию объектов Publisher.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Publisher>>> Index()
    {
        return View(await context.Publishers.ToListAsync());
    }

    /// <summary>
    /// Отображает форму для создания нового издателя.
    /// </summary>
    /// <returns>Представление Create с пустой формой.</returns>
    [HttpGet]
    public IActionResult Create()
    {
        return View("Create");
    }

    /// <summary>
    /// Обрабатывает отправку формы создания нового издателя.
    /// </summary>
    /// <param name="publisher">Объект издателя, переданный из формы.</param>
    /// <returns>
    /// При успешном добавлении выполняет перенаправление на действие Index.
    /// При ошибках валидации возвращает представление Create с текущим объектом publisher и сообщениями об ошибках.
    /// </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Publisher publisher)
    {
        if (ModelState.IsValid)
        {
            context.Publishers.Add(publisher);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(publisher);
    }

    /// <summary>
    /// Отображает страницу подтверждения удаления издателя.
    /// </summary>
    /// <param name="id">Идентификатор издателя.</param>
    /// <returns>
    /// Возвращает представление Delete с данными издателя, если он найден.
    /// Если id не указан или издатель не существует, возвращает HTTP 404 Not Found.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var publisher = await context.Publishers.FindAsync(id);
        if (publisher == null)
            return NotFound();

        return View(publisher);
    }

    /// <summary>
    /// Подтверждает удаление издателя.
    /// </summary>
    /// <param name="id">Идентификатор издателя.</param>
    /// <returns>Перенаправляет на действие Index после удаления (если запись существовала).</returns>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var publisher = await context.Publishers.FindAsync(id);
        if (publisher != null)
        {
            context.Publishers.Remove(publisher);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Отображает форму для редактирования существующего издателя.
    /// </summary>
    /// <param name="id">Идентификатор издателя.</param>
    /// <returns>
    /// Представление Edit с данными издателя для редактирования.
    /// Если id не указан или издатель не найден, возвращает HTTP 404 Not Found.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var publisher = await context.Publishers.FindAsync(id);
        if (publisher == null)
            return NotFound();

        return View(publisher);
    }

    /// <summary>
    /// Обрабатывает отправку формы редактирования издателя.
    /// </summary>
    /// <param name="id">Идентификатор редактируемого издателя (должен совпадать с PublisherId объекта).</param>
    /// <param name="publisher">Объект с обновлёнными данными.</param>
    /// <returns>
    /// При успешном обновлении перенаправляет на Index.
    /// При ошибках валидации возвращает представление Edit с текущим объектом publisher и сообщениями об ошибках.
    /// При конфликте параллельного обновления (DbUpdateConcurrencyException) повторно проверяет существование записи.
    /// </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Publisher publisher)
    {
        if (id != publisher.PublisherId)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(publisher);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(publisher.PublisherId))
                    return NotFound();

                throw; // Если запись существует, но конфликт не устранён, пробрасываем исключение дальше
            }
        }

        return View(publisher);
    }

    /// <summary>
    /// Проверяет, существует ли издатель с указанным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор издателя.</param>
    /// <returns>true, если запись существует; иначе false.</returns>
    private bool PublisherExists(int id) => context.Publishers.Any(e => e.PublisherId == id);
}