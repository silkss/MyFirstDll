using BlazorUi.Services.Base;
using DataLayer;
using DataLayer.Models.Strategies;
using Microsoft.EntityFrameworkCore;

namespace BlazorUi.Services;

public class StrategyRepository : BaseRepository<OptionStrategy>
{
    public StrategyRepository(IDbContextFactory<DataContext> dbContextFactory) : base(dbContextFactory)
    {

    }
    protected override bool _Contains(List<OptionStrategy> entities, OptionStrategy entity)
    {
        return false;
    }
}
