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

    protected override bool _Contains(List<DbFuture> entities, DbFuture entity) => entities.Any(item => item.ConId == entity.ConId);
}
