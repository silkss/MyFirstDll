using DataLayer;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MFDll.Services.Repositories.Base;

internal abstract class Repository<T> where T : class, IEntity
{
    protected List<T> _Items = new();
    protected MFDllContext _DbContext;
    protected DbSet<T> _DbSet;
    public IEnumerable<T> GetAll() => _Items;
    public T? Get(int id) => _Items.FirstOrDefault(i => i.Id == id);

    public Repository(MFDllContext dbContext, DbSet<T> dbset)
    {
        _DbContext = dbContext;
        _DbSet = dbset;
        _Items = _DbSet.ToList();
    }

    public void Add(T item)
    {
        if (item == null) return;
        if (_Items.Contains(item)) return;

        _DbSet.Add(item);
        _DbContext.SaveChanges();
        _Items.Add(item);
    }

    public void Update(int Id, T source)
    {
        var destination = Get(Id);

        if (destination == null) return;

        _Update(destination, source);

        _DbContext.SaveChanges();
    }
    protected abstract void _Update(T destination, T source);
}


