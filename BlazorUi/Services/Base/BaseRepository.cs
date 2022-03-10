using DataLayer;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorUi.Services.Base;

public abstract class BaseRepository<T>
    where T : class, IEntity
{
    //protected readonly DataContext _dataContext;
    protected readonly IDbContextFactory<DataContext> _dataContextFactory;

    public BaseRepository(IDbContextFactory<DataContext> dataContextFactory)
    {
        _dataContextFactory = dataContextFactory;
    }
    public async Task<bool> CreateAsync(T entity)
    {
        using (var _dataContext = _dataContextFactory.CreateDbContext())
        {
            if (entity == null) return false;
            var set = _dataContext.Set<T>();

            if (_Contains(set, entity)) return false;

            set.Add(entity);
            await _dataContext.SaveChangesAsync();
        }
        return true;
    }

    protected abstract bool _Contains(DbSet<T> set, T entity);

    /// <summary>
    /// Есть базовая реализация, но в случае необходимости, например, для подгрузки связанных сущностей, 
    /// сделал виртуальным. 
    /// </summary>
    /// <returns></returns>
    public virtual async Task<IList<T>> GetAllAsync()
    {
        var list = new List<T>();
        using (var _dataContext = _dataContextFactory.CreateDbContext())
        {
            list = await _dataContext.Set<T>().ToListAsync();
        }
        return list;
    }
    public async Task<T?> GetByIdAsync(int id)
    {
        T? entity = null;
        using (var _dataContext = _dataContextFactory.CreateDbContext())
        {
            entity = await _dataContext.Set<T>().FirstOrDefaultAsync(i => i.Id == id);
        }
        return entity;
    }
    public async Task UpdateEntity(T entity)
    {
        using (var _dataContext = _dataContextFactory.CreateDbContext())
        {

            _dataContext.Entry(entity).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
        }
    }
}
