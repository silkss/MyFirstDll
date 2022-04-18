using BlazorUi.Services.Base;
using DataLayer;
using DataLayer.Models.Strategies;
using Microsoft.EntityFrameworkCore;

namespace BlazorUi.Services;

public class ContainersRepository //: BaseRepository<Container>
{
    private readonly IDbContextFactory<DataContext> _dataContextFactory;
    private List<Container> _entities = new();
    public ContainersRepository(IDbContextFactory<DataContext> dataContextFactory)// : base(dataContextFactory)
    {
        _dataContextFactory = dataContextFactory;
    }
    public async Task<IList<Container>> GetAllAsync()
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
    public Container? GetById(int id) => GetAllAsync().Result.SingleOrDefault(c => c.Id == id);
    public async Task<Container?> CreateAsync(Container entity)
    {
        Container? new_entity = null;
        if (entity == null) return null;
        if (_entities.Any(c => c.Account == entity.Account && c.FutureId == entity.FutureId)) return null;

        using (var _dataContext = _dataContextFactory.CreateDbContext())
        {
            _dataContext.Set<Container>().Add(entity);

            await _dataContext.SaveChangesAsync();

            //это необходимо для того, чтобы сразу получить ссылку на родительский инструмент в контейнере
            new_entity = _dataContext.Set<Container>()
                .Include(container => container.Future)
                .SingleOrDefault(cont => cont.Id == entity.Id);
            if (new_entity != null)
                _entities.Add(new_entity);
        }
        return new_entity;
    }

    public async Task UpdateAsync(Container source)
    {
        using (var datacontex = _dataContextFactory.CreateDbContext())
        {
            var target = datacontex.Set<Container>().Single(c => c.Id == source.Id);
            target.WantedPnl = source.WantedPnl;
            target.KeepAliveInDays = source.KeepAliveInDays;
            await datacontex.SaveChangesAsync();
        }
    }
}
