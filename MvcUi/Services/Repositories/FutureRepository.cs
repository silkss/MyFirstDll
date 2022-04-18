using DataLayer;
using DataLayer.Models.Instruments;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MvcUi.Services.Repositories;

public class FutureRepository
{
    private readonly IDbContextFactory<DataContext> _dbContextFactory;
    private List<DbFuture> _dbFutureList;

    public FutureRepository(IDbContextFactory<DataContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public IEnumerable<DbFuture> GetAll()
    {
        if (_dbFutureList == null)
        {
            using (var dbcontext = _dbContextFactory.CreateDbContext())
            {
                _dbFutureList = dbcontext.Set<DbFuture>().ToList();
            }
        }

        return _dbFutureList;
    }
    public DbFuture? GetById(int id) => GetAll().SingleOrDefault(f => f.Id == id);
    public bool DeleteById(int id)
    {
        using (var dbcontext = _dbContextFactory.CreateDbContext())
        {
            var entity = GetById(id);
            if (entity == null) return false;

            dbcontext.Set<DbFuture>().Remove(entity);
            dbcontext.SaveChanges();
        }
        return true;
    }
    public void Add(DbFuture entity)
    {
        using(var dbcontext = _dbContextFactory.CreateDbContext())
        {
            dbcontext.Set<DbFuture>().Add(entity);
            dbcontext.SaveChanges();
        }
    }
}
