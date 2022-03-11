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

    protected override bool _Contains(List<LongStraddle> entities, LongStraddle entity) =>
        entities.Any(e => e.Id == entity.Id);
}
