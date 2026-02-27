using Microsoft.AspNetCore.Mvc;
using LibrariesWebApp.Services;

namespace LibrariesWebApp.Controllers.Base;

/// <summary>
/// Базовый контроллер для управления сущностями (CRUD операции).
/// Использует сервис <see cref="ICrudService{TEntity, TKey}"/> для работы с данными.
/// </summary>
/// <typeparam name="TEntity">Тип сущности, с которой работает контроллер.</typeparam>
/// <typeparam name="TKey">Тип первичного ключа сущности.</typeparam>
public abstract class CrudController<TEntity, TKey> : Controller
    where TEntity : class
{
    /// <summary>
    /// Сервис для CRUD операций над сущностями.
    /// </summary>
    private readonly ICrudService<TEntity, TKey> _service;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера с указанным сервисом.
    /// </summary>
    /// <param name="service">Сервис для работы с сущностями.</param>
    protected CrudController(ICrudService<TEntity, TKey> service)
    {
        _service = service;
    }

    /// <summary>
    /// Отображает страницу со списком всех сущностей.
    /// </summary>
    /// <returns>Представление Index, содержащее коллекцию объектов <typeparamref name="TEntity"/>.</returns>
    [HttpGet]
    public virtual async Task<IActionResult> Index()
    {
        var items = await _service.GetAllAsync();
        return View(items);
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
            await _service.CreateAsync(entity);
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
        var entity = await _service.GetByIdAsync(id);
        return View(entity); // entity гарантированно не null после проверки
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
    /// Если сущность не найдена (например, удалена другим пользователем), возвращает HTTP 404 Not Found.
    /// При других ошибках (например, конфликт конкурентного обновления) исключение пробрасывается дальше.
    /// </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> Edit(TKey id, TEntity entity)
    {
        if (id != null && !id.Equals(GetEntityId(entity)))
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                await _service.UpdateAsync(entity);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex) when (ex.Message == "Сущность не найдена.")
            {
                return NotFound();
            }
            // Другие исключения (например, DbUpdateConcurrencyException) не перехватываются
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
        var entity = await _service.GetByIdAsync(id);
        return View(entity); // entity гарантированно не null после проверки
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
        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Получает значение первичного ключа из сущности.
    /// По умолчанию ищет свойство с именем "Id" или "{EntityName}Id".
    /// При необходимости может быть переопределён в производных классах.
    /// </summary>
    /// <param name="entity">Сущность, из которой извлекается ключ.</param>
    /// <returns>Значение первичного ключа (не может быть null).</returns>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается, если не удалось определить свойство идентификатора
    /// или значение свойства равно null.
    /// </exception>
    protected virtual TKey GetEntityId(TEntity entity)
    {
        var property = typeof(TEntity).GetProperty("Id") ??
                       typeof(TEntity).GetProperty($"{typeof(TEntity).Name}Id");

        if (property == null)
        {
            throw new InvalidOperationException(
                $"Не удалось определить свойство идентификатора для сущности {typeof(TEntity).Name}. " +
                $"Переопределите метод GetEntityId в производном контроллере.");
        }

        var value = property.GetValue(entity);
        if (value == null)
        {
            throw new InvalidOperationException(
                $"Свойство идентификатора '{property.Name}' сущности {typeof(TEntity).Name} равно null.");
        }

        return (TKey)value;
    }
}