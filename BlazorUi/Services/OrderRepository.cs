using BlazorUi.Services.Base;
using DataLayer;
using DataLayer.Models.Strategies;
using Microsoft.EntityFrameworkCore;

namespace BlazorUi.Services;

public class OrderRepository : BaseRepository<DbOrder>
{
    public OrderRepository(IDbContextFactory<DataContext> dbContextFactory) : base(dbContextFactory)
    {

    }
    protected override bool _Contains(List<DbOrder> entities, DbOrder entity) => entities.Contains(entity);
}
