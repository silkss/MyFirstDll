using Connectors.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebUi.Services.Workers;

public class DbWorker
{
    private readonly IConnector<DbFuture, DbOption> _connector;
    private List<int> _reqsList = new();
    public DbWorker(IConnector<DbFuture,DbOption> connector)
    {
        _connector = connector;

        _connector.FutureAdded += onFutureAdded;
        _connector.OptionAdded += onOptionAdded;
    }
    #region Private methods
    private void onFutureAdded(int req, DbFuture future)
    {
        if (_reqsList.Contains(req))
        {
            //_dbcontext.Futures.Add(future);
            //_reqsList.Remove(req);
            //_dbcontext.SaveChanges();
        }
    }
    private void onOptionAdded(int req, DbOption option)
    {
        if (_reqsList.Contains(req))
        {
            _onOptionAdded(option);
            _reqsList.Remove(req);
        } 
    }
    #endregion

    private void _onOptionAdded(DbOption option, [FromServices] DataContext? context = null)
    {
        if (context != null)
        {
            context.Options.Add(option);
            context.SaveChanges();
        }
    }
    public void RequestFuture(string localsymbol)
    {
        _reqsList.Add(_connector.RequestFuture(localsymbol));
    }
}
