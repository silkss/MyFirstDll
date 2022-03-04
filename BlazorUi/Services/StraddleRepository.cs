using BlazorUi.Services.Base;
using DataLayer;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorUi.Services;

public class StraddleRepository : BaseRepository<LongStraddle>
{
    public StraddleRepository(DataContext dataContext) : base(dataContext)
    {

    }

    protected override bool _Contains(DbSet<LongStraddle> set, LongStraddle entity) => set.Find(entity.Id) != null;
}
