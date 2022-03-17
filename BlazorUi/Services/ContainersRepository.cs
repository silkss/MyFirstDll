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
                    .Include(container => container.LongStraddles)
                    .ThenInclude(straddle => straddle.OptionStrategies)
                    .ThenInclude(strategy => strategy.StrategyOrders)
                    .Include(container => container.LongStraddles)
                    .ThenInclude(straddle => straddle.OptionStrategies)
                    .ThenInclude(strategy => strategy.Option)
                    .ToListAsync();
            }
        }
        return _entities;
    }
    public override async Task<Container?> CreateAsync(Container entity)
    {
        Container? new_entity = null;
        using (var _dataContext = _dataContextFactory.CreateDbContext())
        {
            if (entity == null) return null;
            if (_Contains(_entities, entity)) return null;
            _dataContext.Set<Container>().Add(entity);

            await _dataContext.SaveChangesAsync();
            new_entity = _dataContext.Set<Container>()
                .Include(container => container.Future)
                .SingleOrDefault(t => t.Id == entity.Id);
            if (new_entity != null)
                _entities.Add(new_entity);
        }
        return new_entity;
    }

    protected override bool _Contains(List<Container> entities, Container entity) =>
        entities.Any(c => c.Account == entity.Account && c.FutureId == entity.FutureId);
}
