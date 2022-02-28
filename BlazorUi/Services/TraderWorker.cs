using Connectors.Enums;
using Connectors.Interfaces;
using DataLayer.Enums;
using DataLayer.Models;
using DataLayer.Models.Instruments;
using DataLayer.Models.Strategies;

namespace BlazorUi.Services;

public class TraderWorker
{
    private readonly IConnector<DbFuture, DbOption> _connector;
    private TimeSpan _period = new(9, 0, 0, 0);

    public TraderWorker(IConnector<DbFuture, DbOption> connector)
    {
        _connector = connector;
    }

    public async void SignalOnOpen(string symbol, double price)
    {
        var future = _connector.GetCachedFutures().FirstOrDefault(f => f.LocalSymbol == symbol);
        if (future == null) return;

        var best_option_chain = future.OptionChain
            .OrderByDescending(oc => oc.ExpirationDate)
            .Reverse()
            .FirstOrDefault(oc => oc.ExpirationDate > (DateTime.Now + _period));

        if (best_option_chain == null) return;

        var best_strike = best_option_chain.Strikes
            .OrderByDescending(strike => strike)
            .Reverse()
            .FirstOrDefault(strike => strike >= price);

        if (best_strike == default) return;

        var straddle = future.Straddles
            .FirstOrDefault(ls => ls.ExpirationDate == best_option_chain.ExpirationDate && ls.Strike == best_strike);

        if (straddle == null)
        {
            var put = await _connector.RequestOptionAsync(best_option_chain.ExpirationDate, best_strike, OptionType.Put, future);
            var call = await _connector.RequestOptionAsync(best_option_chain.ExpirationDate, best_strike, OptionType.Call, future);

            if (put == null) return;
            if (call == null) return;

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

            straddle = new LongStraddle();

            straddle.OptionStrategies.Add(put_strategy);
            straddle.OptionStrategies.Add(call_strategy);

            future.Straddles.Add(straddle);
        }
    }

    public void SignalOnClose(string symbol, double price)
    { 
    }
}
