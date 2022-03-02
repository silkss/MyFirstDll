using Connectors.Enums;
using Connectors.Interfaces;
using DataLayer.Enums;
using DataLayer.Models;
using DataLayer.Models.Instruments;
using DataLayer.Models.Strategies;
using Microsoft.Extensions.Logging;

namespace BlazorUi.Services;

public class TraderWorker
{
    private readonly IConnector<DbFuture, DbOption> _connector;
    private readonly ILogger<TraderWorker> _logger;
    private TimeSpan _period = new(9, 0, 0, 0);

    public TraderWorker(IConnector<DbFuture, DbOption> connector, ILogger<TraderWorker> logger)
    {
        _connector = connector;
        _logger = logger;
    }

    public void SignalOnOpen(string symbol, double price)
    {
        var future = _connector.GetCachedFutures().FirstOrDefault(f => f.LocalSymbol == symbol);
        if (future == null)
        {
            _logger.LogError($"Cant find cached future with symbol {symbol}");
            return;
        }

        var best_option_chain = future.OptionChain
            .OrderByDescending(oc => oc.ExpirationDate)
            .Reverse()
            .FirstOrDefault(oc => oc.ExpirationDate > (DateTime.Now + _period));

        if (best_option_chain == null)
        {
            _logger.LogError($"Cant find option chain with expiration data more then {DateTime.Now + _period}");
            return;
        }

        var best_strike = best_option_chain.Strikes
            .OrderByDescending(strike => strike)
            .Reverse()
            .FirstOrDefault(strike => strike >= price);

        if (best_strike == default)
        {
            _logger.LogError($"cant find best strike more then {price}");
            return;
        }

        var straddle = future.Straddles
            .FirstOrDefault(ls => ls.ExpirationDate == best_option_chain.ExpirationDate && ls.Strike == best_strike);

        if (straddle == null)
        {
            _logger.LogInformation($"No straddle with exp:{best_option_chain.ExpirationDate.ToShortDateString()} and strike:{best_strike}");

            var put =  _connector.RequestOptionAsync(best_option_chain.ExpirationDate, best_strike, OptionType.Put, future);
            if (put == null)
            {
                _logger.LogError("Impossible to request Put");
                return;
            }
            _logger.LogInformation("Requested Put");

            var call =  _connector.RequestOptionAsync(best_option_chain.ExpirationDate, best_strike, OptionType.Call, future);
            if (call == null)
            {
                _logger.LogError("Impossible to request Call");
                return;
            }
            _logger.LogInformation("Requsted Call");

            var put_strategy = new OptionStrategy
            {
                Direction = Direction.Buy,
                Volume = 1,
                Option = put,
                StrategyLogic = StrategyLogic.OpenPoition,
            };

            var call_strategy = new OptionStrategy
            {
                Direction = Direction.Buy,
                Volume = 1,
                Option = call,
                StrategyLogic = StrategyLogic.OpenPoition,
            };

            straddle = new LongStraddle
            {
                ExpirationDate = best_option_chain.ExpirationDate,
                Strike = best_strike,
            };

            straddle.OptionStrategies.Add(put_strategy);
            straddle.OptionStrategies.Add(call_strategy);

            _logger.LogInformation($"Created strddle for {symbol} with price {straddle.Strike}");
            future.Straddles.Add(straddle);
        }
    }

    public void SignalOnClose(string symbol, double price)
    { 
    }
}
