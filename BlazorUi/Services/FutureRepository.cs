using BlazorUi.Services.Base;
using DataLayer;
using DataLayer.Models.Instruments;
using Microsoft.EntityFrameworkCore;

namespace BlazorUi.Services;

public class FutureRepository : BaseRepository<DbFuture>
{
    public FutureRepository(DataContext dataContext) : base(dataContext)
    {
    }

    protected override bool _Contains(DbSet<DbFuture> set, DbFuture entity) => set.Any(item => item.ConId == entity.ConId);
}
