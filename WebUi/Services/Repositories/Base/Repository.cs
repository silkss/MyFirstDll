using System;
using System.Threading.Tasks;
using WebUi.Services.Repositories.Interfaces;

namespace WebUi.Services.Repositories.Base;

public class Repository<T> : IRepository<T> where T: class
{
    private readonly DataContext _dataContext;

    public Repository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task CreateAsync(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
        _dataContext.Add(item);
        await _dataContext.SaveChangesAsync();
    }
}

