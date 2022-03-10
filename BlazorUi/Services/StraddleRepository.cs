using BlazorUi.Services.Base;
using DataLayer;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorUi.Services;

public class StraddleRepository : BaseRepository<LongStraddle>
{
    public StraddleRepository(IDbContextFactory<DataContext> dataContextFactory) : base(dataContextFactory)
    {

    }

    protected override bool _Contains(DbSet<LongStraddle> set, LongStraddle entity) => set.Find(entity.Id) != null;
}
