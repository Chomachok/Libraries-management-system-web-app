using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibrariesWebApp.Data;
using LibrariesWebApp.Models;

namespace LibrariesWebApp.Controllers;

/// <summary>
/// Контроллер для управления тематическими рубриками книг (CRUD операции).
/// Обеспечивает отображение списка, создание, редактирование и удаление записей об рубриках книг.
/// </summary>

public class SubjectsController(AppDbContext context) : Controller
{
    /// <summary>
    /// Отображает страницу со списком всех рубрик книг.
    /// </summary>
    /// <returns>Представление Index, содержащее коллекцию объектов Subject.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Subject>>> Index()
    {
        return await context.Subjects.ToListAsync();
    }
    
    /// <summary>
    /// Отображает форму для создания новой рубрики книг
    /// </summary>
    /// <returns>Представление Create с пустой формой.</returns>
    [HttpGet]
    public IActionResult Create()
    {
        return View("Create");
    }
    
    /// <summary>
    /// Обрабатывает отправку формы создания новой рубрики книг.
    /// </summary>
    /// <param name="subject">Объект рубрики книг, переданный из формы.</param>
    /// <returns>
    /// При успешном добавлении выполняет перенаправление на действие Index.
    /// При ошибках валидации возвращает представление Index (с текущим списком рубрик книг и сообщениями об ошибках).
    /// </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(Subject subject)
    {
        if (ModelState.IsValid)
        {
            context.Subjects.Add(subject);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        return View("Index");
    }

    /// <summary>
    /// Отображает страницу подтверждения удаления рубрики книг.
    /// </summary>
    /// <param name="id"> Идентификатор рубрики книг.</param>
    /// <returns>
    /// Возвращает представление Delete с данными рубрики книг, если она найдена.
    /// Если id не указан или рубрика книг не существует, возвращается HTTP 404 Not Found.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var subject = await context.Subjects.FindAsync(id);
        if (subject == null)
            return NotFound();
        
        return View(subject);
    }

    /// <summary>
    /// Подтверждает удаление рубрики книг.
    /// </summary>
    /// <param name="id">Идентификатор рубрики книг.</param>
    /// <returns>Перенаправляет на действие Index после удаления (если запись существовала).</returns>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var subject = await context.Subjects.FindAsync(id);
        
        if (subject != null)
        {
            context.Subjects.Remove(subject);
            await context.SaveChangesAsync();
        }
        
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Отображает форму для редактирования существующей рубрики книг.
    /// </summary>
    /// <param name="id">Идентификатор рубрики книг.</param>
    /// <returns>
    /// Представление Edit с данными о рубрике книг для редактирования.
    /// Если id не указан или рубрика не найдена, возвращает HTTP 404 Not Found.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var subject = await context.Subjects.FindAsync(id);
        if (subject == null)
            return NotFound();

        return View(subject);
    }

    /// <summary>
    /// Обрабатывает отправку формы редактирования рубрики книг.
    /// </summary>
    /// <param name="id">Идентификатор рубрики книг.</param>
    /// <param name="subject">Объект с обновлёнными данными.</param>
    /// <returns>
    /// При успешном обновлении перенаправляет на Index.
    /// При ошибках валидации возвращает представление Edit с текущим объектом subject и сообщениями об ошибках.
    /// При конфликте параллельного обновления (DbUpdateConcurrencyException) повторно проверяет существование записи.
    /// </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Subject subject)
    {
        if (id != subject.SubjectId)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(subject);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(subject.SubjectId))
                    return NotFound();

                throw;
            }
        }

        return View(subject);
    }
    
    private bool SubjectExists(int id) =>  context.Subjects.Any(e => e.SubjectId == id);
}