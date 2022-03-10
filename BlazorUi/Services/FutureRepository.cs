using BlazorUi.Services.Base;
using DataLayer;
using DataLayer.Models.Instruments;
using Microsoft.EntityFrameworkCore;

namespace BlazorUi.Services;

public class FutureRepository : BaseRepository<DbFuture>
{
    public FutureRepository(IDbContextFactory<DataContext> dataContextFactory) : base(dataContextFactory)
    {
    }

    protected override bool _Contains(DbSet<DbFuture> set, DbFuture entity) => set.Any(item => item.ConId == entity.ConId);
}
