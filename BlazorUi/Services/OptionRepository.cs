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

    #region Methods
    #region _ProtectedMethods
    protected override bool _Contains(DbSet<DbOption> set, DbOption entity) => set.Any(item => item.ConId == entity.ConId);
    #endregion

    #region PublicMethods
    public DbOption? GetOptionBuyConId(int conid) => _dataContext.Set<DbOption>()
        .FirstOrDefault(option => option.ConId == conid);
    #endregion
    #endregion
}
