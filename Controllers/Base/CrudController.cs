using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibrariesWebApp.Data;

namespace LibrariesWebApp.Controllers.Base;

/// <summary>
/// Базовый контроллер для управления сущностями (CRUD операции).
/// Обеспечивает отображение списка, создание, редактирование и удаление записей 
/// для любой сущности, унаследованной от данного класса.
/// </summary>
/// <typeparam name="TEntity">Тип сущности, с которой работает контроллер.</typeparam>
/// <typeparam name="TKey">Тип первичного ключа сущности.</typeparam>
public abstract class CrudController<TEntity, TKey> : Controller
    where TEntity : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера с контекстом базы данных.
    /// </summary>
    /// <param name="context">Контекст базы данных приложения.</param>
    protected CrudController(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    /// <summary>
    /// Отображает страницу со списком всех сущностей.
    /// </summary>
    /// <returns>Представление Index, содержащее коллекцию объектов <typeparamref name="TEntity"/>.</returns>
    [HttpGet]
    public virtual async Task<IActionResult> Index()
    {
        return View(await _dbSet.ToListAsync());
    }

    /// <summary>
    /// Отображает форму для создания новой сущности.
    /// </summary>
    /// <returns>Представление Create с пустой формой.</returns>
    [HttpGet]
    public virtual IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Обрабатывает отправку формы создания новой сущности.
    /// </summary>
    /// <param name="entity">Объект сущности, переданный из формы.</param>
    /// <returns>
    /// При успешном добавлении выполняет перенаправление на действие Index.
    /// При ошибках валидации возвращает представление Create с текущим объектом 
    /// и сообщениями об ошибках.
    /// </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> Create(TEntity entity)
    {
        if (ModelState.IsValid)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(entity);
    }

    /// <summary>
    /// Отображает форму для редактирования существующей сущности.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>
    /// Представление Edit с данными сущности для редактирования.
    /// Если id не указан или сущность не найдена, возвращает HTTP 404 Not Found.
    /// </returns>
    [HttpGet]
    public virtual async Task<IActionResult> Edit(TKey id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
            return NotFound();

        return View(entity);
    }

    /// <summary>
    /// Обрабатывает отправку формы редактирования сущности.
    /// </summary>
    /// <param name="id">Идентификатор сущности (из маршрута).</param>
    /// <param name="entity">Объект с обновлёнными данными.</param>
    /// <returns>
    /// При успешном обновлении перенаправляет на Index.
    /// При ошибках валидации возвращает представление Edit с текущим объектом 
    /// и сообщениями об ошибках.
    /// При конфликте параллельного обновления (DbUpdateConcurrencyException) 
    /// повторно проверяет существование записи.
    /// </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> Edit(TKey id, TEntity entity)
    {
        if (!id.Equals(GetEntityId(entity)))
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EntityExists(id))
                    return NotFound();
                throw;
            }
        }
        return View(entity);
    }

    /// <summary>
    /// Отображает страницу подтверждения удаления сущности.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>
    /// Возвращает представление Delete с данными сущности, если она найдена.
    /// Если id не указан или сущность не существует, возвращается HTTP 404 Not Found.
    /// </returns>
    [HttpGet]
    public virtual async Task<IActionResult> Delete(TKey id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
            return NotFound();

        return View(entity);
    }

    /// <summary>
    /// Подтверждает удаление сущности.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>Перенаправляет на действие Index после удаления (если запись существовала).</returns>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> DeleteConfirmed(TKey id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Получает значение первичного ключа из сущности.
    /// По умолчанию ищет свойство с именем "Id" или "{EntityName}Id".
    /// При необходимости может быть переопределён в производных классах.
    /// </summary>
    /// <param name="entity">Сущность, из которой извлекается ключ.</param>
    /// <returns>Значение первичного ключа.</returns>
    protected virtual TKey GetEntityId(TEntity entity)
    {
        var property = typeof(TEntity).GetProperty("Id") ?? 
                       typeof(TEntity).GetProperty($"{typeof(TEntity).Name}Id");
        return (TKey)property?.GetValue(entity);
    }

    /// <summary>
    /// Проверяет, существует ли сущность с указанным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>true, если сущность существует; иначе false.</returns>
    protected virtual async Task<bool> EntityExists(TKey id)
    {
        return await _dbSet.FindAsync(id) != null;
    }
}