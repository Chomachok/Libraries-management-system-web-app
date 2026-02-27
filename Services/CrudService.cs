using Microsoft.EntityFrameworkCore;
using LibrariesWebApp.Data;

namespace LibrariesWebApp.Services;

/// <summary>
/// Обобщённая реализация CRUD сервиса с использованием Entity Framework Core.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
/// <typeparam name="TKey">Тип первичного ключа.</typeparam>
public class CrudService<TEntity, TKey> : ICrudService<TEntity, TKey>
    where TEntity : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    /// <summary>
    /// Инициализирует новый экземпляр сервиса.
    /// </summary>
    /// <param name="context">Контекст базы данных приложения.</param>
    public CrudService(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    /// <inheritdoc />
    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <inheritdoc />
    public virtual async Task<TEntity> GetByIdAsync(TKey id)
    {
        return (await _dbSet.FindAsync(id) ?? null) ?? throw new InvalidOperationException();
    }

    /// <inheritdoc />
    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc />
    public virtual async Task UpdateAsync(TEntity entity)
    {
        try
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            var id = GetEntityId(entity);
            if (!await ExistsAsync(id))
                throw new InvalidOperationException("Сущность не найдена.");
            throw; // Если существует, значит конфликт версий — пробрасываем дальше
        }
    }

    /// <inheritdoc />
    public virtual async Task DeleteAsync(TKey id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public virtual async Task<bool> ExistsAsync(TKey id)
    {
        return await _dbSet.FindAsync(id) != null;
    }

    /// <summary>
    /// Получает значение первичного ключа из сущности.
    /// Используется внутри сервиса для проверок.
    /// </summary>
    /// <param name="entity">Сущность, из которой извлекается ключ.</param>
    /// <returns>Значение первичного ключа.</returns>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается, если не удалось определить свойство идентификатора.
    /// </exception>
    protected virtual TKey GetEntityId(TEntity entity)
    {
        var property = typeof(TEntity).GetProperty("Id") ??
                       typeof(TEntity).GetProperty($"{typeof(TEntity).Name}Id");
    
        if (property == null)
            throw new InvalidOperationException(
                $"Не удалось определить свойство идентификатора для сущности {typeof(TEntity).Name}.");

        var value = property.GetValue(entity);
    
        if (value == null)
            throw new InvalidOperationException(
                $"Значение идентификатора для сущности {typeof(TEntity).Name} равно null.");

        return (TKey)value;
    }
}