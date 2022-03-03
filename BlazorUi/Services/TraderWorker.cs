using Connectors.Interfaces;
using Connectors.Models.Instruments;
using DataLayer.Models.Instruments;
using DataLayer.Models.Strategies;

namespace BlazorUi.Services;

public class TraderWorker
{
    #region Props

    #region _privateProps
    private readonly IConnector _connector;
    private readonly ILogger<TraderWorker> _logger;

    private readonly List<Container> _workingContainers = new();

    private TimeSpan _period = new(9, 0, 0, 0);
    #endregion
    
    #endregion

    public TraderWorker(IConnector connector, ILogger<TraderWorker> logger)
    {
        _connector = connector;
        _logger = logger;
    }
    #region Methods

    #region privateMethods
    private Container? getContainer(string symbol, string account) =>
        _workingContainers.FirstOrDefault(c => c.Future.LocalSymbol == symbol && c.Account == account);
    private OptionChain? getBestOptionChain(DbFuture future) => future.OptionChain
        .OrderBy(oc => oc.ExpirationDate)
        .FirstOrDefault(opt_chain => opt_chain.ExpirationDate > (DateTime.Now + _period));
    private double getBestStrike(OptionChain optionChain, double price) => optionChain.Strikes
        .OrderBy(s => s)
        .FirstOrDefault(s => s > price);
    #endregion

    #region PublicMethods
    public void StartContainer(Container container)
    {
        if (!_workingContainers.Contains(container))
        {
            _workingContainers.Add(container);
        }
        _connector.CacheFuture(container.Future);
        container.Start();
    }

    public void StopContainer(Container container)
    {
        if (_workingContainers.Contains(container))
        {
            _workingContainers.Remove(container);
        }
        container.Stop();
    }

    public void SignalOnOpen(string symbol, double price, string account)
    {
        if (_connector.IsConnected == false) return;

        var container = getContainer(symbol, account);
        if (container == null || container.Future == null)
        {
            _logger.LogError($"No opened containers with symbol {symbol} and account {account}. Or this container dont have future");
            return;
        }

        var best_option_chain = getBestOptionChain(container.Future);
        if (best_option_chain == null)
        {
            _logger.LogError("Cant find best option chain!");
            return;
        }

        var best_strike = getBestStrike(best_option_chain, price);
        if (best_strike == default(double))
        {
            _logger.LogError($"Cant find best strike for price {price}.");
            return;
        }


        _logger.LogInformation($"best option {best_option_chain.ExpirationDate} with best strike {best_strike}");
    }

    public void SignalOnClose(string symbol, double price)
    { 
    }
    #endregion

    #endregion
}
