using BlazorUi.Services.Base;
using DataLayer;
using DataLayer.Models.Strategies;
using Microsoft.EntityFrameworkCore;

namespace BlazorUi.Services;

public class ContainersRepository : BaseRepository<Container>
{
    public ContainersRepository(IDbContextFactory<DataContext> dataContextFactory) : base(dataContextFactory)
    {

    }
    public override async Task<IList<Container>> GetAllAsync()
    {
        var list = new List<Container>();
        using (var _dataContext = _dataContextFactory.CreateDbContext())
        {
            list = await _dataContext
                .Containers
                .Include(container => container.Future)
                .ThenInclude(future => future.Options)
                .Include(container => container.LongStraddles)
                .ThenInclude(straddle => straddle.OptionStrategies)
                .ToListAsync();
        }
        return list;
    }
    protected override bool _Contains(DbSet<Container> set, Container entity) => 
        set.Any(c => c.Account == entity.Account && c.LastTradeDate == entity.LastTradeDate);
}
