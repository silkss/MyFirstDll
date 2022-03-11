using BlazorUi.Services.Base;
using DataLayer;
using DataLayer.Models.Instruments;
using Microsoft.EntityFrameworkCore;

namespace BlazorUi.Services;

public class OptionRepository : BaseRepository<DbOption>
{
    public OptionRepository(IDbContextFactory<DataContext> dataContextFactory) : base(dataContextFactory)
    {

    }

    #region Methods
    #region _ProtectedMethods
    protected override bool _Contains(List<DbOption> entities, DbOption entity) => entities.Any(item => item.ConId == entity.ConId);
    #endregion

    #region PublicMethods
    public DbOption? GetOptionBuyConId(int conid)
    {
        DbOption? option = null;
        using (var _dataContext = _dataContextFactory.CreateDbContext())
        {
            option = _dataContext.Set<DbOption>()
                .FirstOrDefault(option => option.ConId == conid);
        }
        return option;
    }
    
    #endregion
    #endregion
}
