using BlazorUi.Services.Base;
using DataLayer;
using DataLayer.Models.Strategies;
using Microsoft.EntityFrameworkCore;

namespace BlazorUi.Services;

public class ContainersRepository : BaseRepository<Container>
{
    public ContainersRepository(DataContext dataContext) : base(dataContext)
    {

    }

    protected override bool _Contains(DbSet<Container> set, Container entity) =>
        set.Any(c => c.Account == entity.Account && c.Future.LocalSymbol == entity.Future.LocalSymbol);
}
