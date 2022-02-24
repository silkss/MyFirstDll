using DataLayer;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorUi.Services.Base;

public abstract class BaseRepository<T>
    where T : class, IEntity
{
    private readonly DataContext _dataContext;

    public BaseRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<bool> CreateAsync(T entity)
    {
        var set = _dataContext.Set<T>();
        
        if (_Contains(set, entity)) return false;

        set.Add(entity);
        await _dataContext.SaveChangesAsync();
        return true;
    }

    protected abstract bool _Contains(DbSet<T> set, T entity);
    public async Task<IList<T>> GetAllAsync() => await _dataContext.Set<T>().ToListAsync();
    public async Task<T?> GetByIdAsync(int id) => await _dataContext.Set<T>().FirstOrDefaultAsync(i => i.Id == id);
}
