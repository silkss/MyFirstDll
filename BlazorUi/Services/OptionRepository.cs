using BlazorUi.Services.Base;
using DataLayer;
using DataLayer.Models.Instruments;
using Microsoft.EntityFrameworkCore;

namespace BlazorUi.Services;

public class OptionRepository : BaseRepository<DbOption>
{
    public OptionRepository(DataContext dataContext) : base(dataContext)
    {

    }

    protected override bool _Contains(DbSet<DbOption> set, DbOption entity) => set.Any(item => item.ConId == entity.ConId);
}
