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
        if (_entities.Count == 0)
        {
            using (var _dataContext = _dataContextFactory.CreateDbContext())
            {
                _entities = await _dataContext
                    .Containers
                    .Include(container => container.Future)
                    .ThenInclude(future => future.Options)
                    .Include(container => container.LongStraddles)
                    .ThenInclude(straddle => straddle.OptionStrategies)
                    .ToListAsync();
            }
        }
        return _entities;
    }
    protected override bool _Contains(List<Container> entities, Container entity) =>
        entities.Any(c => c.Account == entity.Account && c.LastTradeDate == entity.LastTradeDate);
}
