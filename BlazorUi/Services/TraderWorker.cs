using Connectors.Enums;
using Connectors.Interfaces;
using Connectors.Models.Instruments;
using DataLayer.Enums;
using DataLayer.Models;
using DataLayer.Models.Instruments;
using DataLayer.Models.Strategies;

namespace BlazorUi.Services;

public class TraderWorker
{
    #region Props

    #region _privateProps
    private readonly IConnector _connector;
    private readonly ILogger<TraderWorker> _logger;
    private readonly OptionRepository _optionRepository;
    private readonly StraddleRepository _straddleRepository;
    private readonly StrategyRepository _strategyRepository;
    private readonly OrderRepository _orderRepository;
    private readonly List<Container> _workingContainers = new();
    private TimeSpan _period = new(9, 0, 0, 0);
    #endregion
    
    #endregion

    public TraderWorker(IConnector connector, 
        ILogger<TraderWorker> logger, 
        OptionRepository optionRepository, StraddleRepository straddleRepository,
        StrategyRepository strategyRepository, OrderRepository orderRepository)
    {
        _connector = connector;
        _logger = logger;
        _optionRepository = optionRepository;
        _straddleRepository = straddleRepository;
        _strategyRepository = strategyRepository;
        _orderRepository = orderRepository;
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

    private async Task createNewStraddleAsync(LongStraddle straddle, Container container)
    {
        DbOption put = new()
        {
            LastTradeDate = straddle.ExpirationDate,
            Strike = (decimal)straddle.Strike,
            OptionType = OptionType.Put,
        };

        if (_connector.TryRequestOption(put, container.Future) == false)
        {
            _logger.LogError("Cant request Put");
            return;
        }

        DbOption call = new()
        {
            LastTradeDate = straddle.ExpirationDate,
            Strike = (decimal)straddle.Strike,
            OptionType = OptionType.Call,
        };

        if (_connector.TryRequestOption(call, container.Future) == false)
        {
            _logger.LogError("cant requst Call");
            return;
        }

        #region проверка на то, есть ли в базе данных опцион. Если да, то используем его
        var db_put = _optionRepository.GetOptionBuyConId(put.ConId);
        if (db_put == null)
            _ = await _optionRepository.CreateAsync(put);
        else
            put = db_put;

        var db_call = _optionRepository.GetOptionBuyConId(call.ConId);
        if (db_call == null)
            _ = await _optionRepository.CreateAsync(call);
        else
            call = db_call;
        #endregion

        await _straddleRepository.CreateAsync(straddle);

        var put_strategy = await straddle.CreatAndAddStrategyAsync(put, straddle.Id, _strategyRepository);
        put_strategy.Start(_connector,_strategyRepository, _orderRepository);

        var call_strategy = await straddle.CreatAndAddStrategyAsync(call, straddle.Id, _strategyRepository);
        call_strategy.Start(_connector, _strategyRepository, _orderRepository);

        container.AddStraddle(straddle);
    }
    #endregion

    #region PublicMethods

    public void StartContainer(Container container)
    {
        if (!_workingContainers.Contains(container))
        {
            _workingContainers.Add(container);
        }
        container.Start(_connector, _straddleRepository, _strategyRepository, _orderRepository);
    }

    public void StopContainer(Container container)
    {
        if (_workingContainers.Contains(container))
        {
            _workingContainers.Remove(container);
        }
        container.Stop();
    }

    public async Task SignalOnOpenAsync(string symbol, double price, string account)
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

        var straddle = container.HasStraddleInCollection(best_option_chain.ExpirationDate, best_strike);
        if (straddle == null)
        {
            container.LongStraddles.ForEach(ls => ls.StraddleLogic = StrategyLogic.ClosePostion);
            straddle = new LongStraddle
            {
                ExpirationDate = best_option_chain.ExpirationDate,
                Strike = best_strike,
                ContainerId = container.Id,
            };
            await createNewStraddleAsync(straddle, container);
            return;
        }
        else if (straddle.StraddleLogic == StrategyLogic.OpenPoition)
        {
            return;
        }
        else if (straddle.StraddleLogic == StrategyLogic.ClosePostion)
		{
            straddle.StraddleLogic = StrategyLogic.ClosePostion;
		}
    }

    public void SignalOnClose(string symbol, double price, string account)
    {
        if (_connector.IsConnected == false) return;

        var container = getContainer(symbol, account);
        if (container == null || container.Future == null)
        {
            _logger.LogError($"No opened containers with symbol {symbol} and account {account}. Or this container dont have future");
            return;
        }

        container.LongStraddles.ForEach(ls => ls.StraddleLogic = StrategyLogic.ClosePostion);
    }
    #endregion

    #endregion
}
