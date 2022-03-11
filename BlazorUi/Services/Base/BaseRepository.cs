using DataLayer;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorUi.Services.Base;

public abstract class BaseRepository<T> : IRepository<T>
    where T : class, IEntity
{
    #region _privateProps
    #endregion

    #region _protectedProps
    protected readonly IDbContextFactory<DataContext> _dataContextFactory;
    protected List<T> _entities = new();
    #endregion

    public BaseRepository(IDbContextFactory<DataContext> dataContextFactory)
    {
        _dataContextFactory = dataContextFactory;
    }

    public async Task<bool> CreateAsync(T entity)
    {
        using (var _dataContext = _dataContextFactory.CreateDbContext())
        {
            if (entity == null) return false;
            if (_Contains(_entities, entity)) return false;
            var set = _dataContext.Set<T>();


            set.Add(entity);
            await _dataContext.SaveChangesAsync();
            _entities.Add(entity);
        }
        return true;
    }

    protected abstract bool _Contains(List<T> entities, T entity);

    /// <summary>
    /// Есть базовая реализация, но в случае необходимости, например, для подгрузки связанных сущностей, 
    /// сделал виртуальным. 
    /// </summary>
    /// <returns></returns>
    public virtual async Task<IList<T>> GetAllAsync()
    {
        if (_entities.Count() == 0)
        {
            using (var _dataContext = _dataContextFactory.CreateDbContext())
            {
                _entities = await _dataContext.Set<T>().ToListAsync();
            }
        }
        return _entities;
    }
    public T? GetById(int id) => _entities.FirstOrDefault(e => e.Id == id);
    public async Task UpdateAsync(T entity)
    {
        using (var _dataContext = _dataContextFactory.CreateDbContext())
        {
            _dataContext.Entry(entity).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
        }
    }
}
