namespace LibrariesWebApp.Services;

/// <summary>
/// Обобщённый интерфейс сервиса для CRUD операций.
/// </summary>
/// <typeparam name="TEntity">Тип сущности, с которой работает сервис.</typeparam>
/// <typeparam name="TKey">Тип первичного ключа сущности.</typeparam>
public interface ICrudService<TEntity, TKey> where TEntity : class
{
    /// <summary>
    /// Возвращает список всех сущностей.
    /// </summary>
    /// <returns>Список всех сущностей.</returns>
    Task<List<TEntity>> GetAllAsync();

    /// <summary>
    /// Возвращает сущность по её идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>Сущность, если найдена; иначе — null.</returns>
    Task<TEntity> GetByIdAsync(TKey id);

    /// <summary>
    /// Создаёт новую сущность.
    /// </summary>
    /// <param name="entity">Объект сущности для создания.</param>
    /// <returns>Созданная сущность с присвоенным идентификатором.</returns>
    Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    /// Обновляет существующую сущность.
    /// </summary>
    /// <param name="entity">Объект сущности с обновлёнными данными.</param>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается, если сущность не найдена в базе данных или произошёл конфликт конкурентного обновления.
    /// </exception>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Удаляет сущность по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>Задача, представляющая асинхронную операцию удаления.</returns>
    Task DeleteAsync(TKey id);

    /// <summary>
    /// Проверяет, существует ли сущность с указанным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>True, если сущность существует; иначе false.</returns>
    Task<bool> ExistsAsync(TKey id);
}