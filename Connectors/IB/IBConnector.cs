using Connectors.Enums;
using Connectors.IB.Enums;
using Connectors.Interfaces;
using IBApi;
using System.Globalization;
using Connectors.Models.Instruments;
using Microsoft.Extensions.Logging;

namespace Connectors.IB;

public class IBConnector : DefaultEWrapper, IConnector
{
    #region _privateProps
    private string ip;
    private int port;
    private int clientid;
    private int nextOrderId;

    private IbDataTypes mktDataType;

    private readonly ILogger _logger;
    private readonly EReaderSignal Signal;
    private readonly EClientSocket ClientSocket;

    private readonly object _futureLock = new();
    private readonly object _optionLock = new();
    private readonly Queue<(int, ContractDetails?)> _futureQueue = new();
    private readonly Queue<(int, ContractDetails?)> _optionQueue = new();
    private readonly string[] _exchanges = { "GLOBEX", "NYMEX" };

    private object _optionCollectionLock = new();
    #endregion

    public Action<int, IFuture> FutureAdded { get; set; } = delegate { };
    public Action<int, IOption> OptionAdded { get; set; } = delegate { };

    public bool IsConnected
    {
        get { return ClientSocket.IsConnected(); }
    }

    public IEnumerable<string> GetExchangeList() => _exchanges;
    
    public IBConnector(ILogger<IBConnector> logger)
    {
        _logger = logger;

        var config = new IBConfig();
        ip = config.Ip;
        port = config.Port;
        clientid = config.ClientId;
        mktDataType = config.DataType;

        Signal = new EReaderMonitorSignal();
        ClientSocket = new EClientSocket(this, Signal);

        _logger.LogInformation("Connector created");
    }

    public void Connect()
    {
        _logger.LogInformation("Connecting...");

        ClientSocket.eConnect(ip, port, clientid);
        var reader = new EReader(ClientSocket, Signal);
        reader.Start();

        new Thread(() =>
        {
            while (ClientSocket.IsConnected())
            {
                Signal.waitForSignal();
                reader.processMsgs();
            }
        })
        { IsBackground = true }.Start();

        ClientSocket.reqMarketDataType(mktDataType switch
        {
            IbDataTypes.Live => 1,
            IbDataTypes.Frozen => 2,
            IbDataTypes.Delayed => 3,
            IbDataTypes.DelayedFrozen => 4,
            _ => 3,
        });

        ClientSocket.reqManagedAccts();

        ClientSocket.reqIds(-1);

        CachedFutures.ForEach(future =>
        {
            ClientSocket.reqSecDefOptParams(future.ConId, future.Symbol, future.Echange, "FUT", future.ConId);
            ClientSocket.reqMktData(future.ConId, future.ToIbContract(), string.Empty, false, false, null);
        });

        CachedOptions.ForEach(option => ClientSocket.reqMktData(option.ConId, option.ToIbContract(), string.Empty, false, false, null));
    }

    public void Disconnect()
    {
        ClientSocket.eDisconnect();
        _logger.LogInformation($"Disconnected at {DateTime.Now}");
    }

    public override void managedAccounts(string accountsList)
    {
        foreach (var acc in accountsList.Trim().Split(','))
        {
            if (!AccountList.Contains(acc))
                AccountList.Add(acc);
        }
    }
    public override void nextValidId(int orderId)
    {
        nextOrderId = orderId;
    }


    #region Instruments
    private readonly List<IFuture> CachedFutures = new();
    private readonly List<IOption> CachedOptions = new();
    private readonly List<string> AccountList = new();
    private readonly List<IOrder> OpenOrders = new();
    private readonly List<int> instrumentreqlist = new();
    public IEnumerable<string> GetAccountList() => AccountList;
    public IEnumerable<IFuture> GetCachedFutures() => CachedFutures;
    public IEnumerable<IOption> GetCachedOptions() => CachedOptions;

    public int RequestFuture(string localSymbol, string exchange)
    {
        var contract = new Contract()
        {
            LocalSymbol = localSymbol,
            SecType = "FUT",
            Exchange = exchange,
            Currency = "USD"
        };
        var orderid = nextOrderId++;
        ClientSocket.reqContractDetails(orderid, contract);
        return orderid;
    }
    public bool TryRequestFuture(IFuture future, string exchange)
    {
        if (!ClientSocket.IsConnected())
            return false;

        int req = RequestFuture(future.LocalSymbol, exchange);

        ContractDetails? contractDetails = null;
        try
        {
            lock (_futureLock)
            {
                while (!_futureQueue.TryPeek(out var tuple) && tuple.Item1 != req)
                {
                    Monitor.Wait(_futureLock);
                }
                contractDetails = _futureQueue.Dequeue().Item2;
            }
        }
        catch (InvalidOperationException)
        {
            return false;
        }

        if (contractDetails == null)
            return false;

        future.MinTick = Convert.ToDecimal(contractDetails.MinTick);
        future.ConId = contractDetails.Contract.ConId;
        future.LocalSymbol = contractDetails.Contract.LocalSymbol;
        future.Symbol = contractDetails.Contract.Symbol;
        future.Echange = contractDetails.Contract.Exchange;
        future.Currency = contractDetails.Contract.Currency;
        future.Multiplier = int.Parse(contractDetails.Contract.Multiplier);
        future.LastTradeDate = DateTime.ParseExact(contractDetails.Contract.LastTradeDateOrContractMonth, "yyyyMMdd", CultureInfo.CurrentCulture);
        future.InstumentType = InstumentType.Future;

        CacheFuture(future);

        return true;
    }
    public int RequestOption(DateTime LastTradeDate, decimal Strike, OptionType type, IFuture parent)
    {
        var contract = new Contract()
        {
            Symbol = parent.Symbol,
            SecType = "FOP",
            Exchange = parent.Echange,
            Currency = parent.Currency,
            LastTradeDateOrContractMonth = LastTradeDate.ToString("yyyyMMdd"),
            Strike = Convert.ToDouble(Strike),
            Right = type == OptionType.Call ? "C" : "P",
            Multiplier = parent.Multiplier.ToString()
        };
        var orderid = nextOrderId++;

        ClientSocket.reqContractDetails(orderid, contract);

        return orderid;
    }

    /// <summary>
    /// For this method you need 
    /// DateTime LastTradeDate, double strike, OptionType type, IFuture parent)
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public bool TryRequestOption(IOption option, IFuture parent)
    {
        if (option == null) return false;

        if (!ClientSocket.IsConnected())
            return false;

        int req = RequestOption(option.LastTradeDate, option.Strike, option.OptionType, parent);
        ContractDetails? contractDetails = null;
        try
        {
            lock (_optionLock)
            {
                while (_optionQueue.Count == 0 || _optionQueue.Peek().Item1 != req)
                {
                    Monitor.Wait(_optionLock);
                }
                contractDetails = _optionQueue.Dequeue().Item2;
            }
        }
        catch (InvalidOperationException)
        {
            return false;
        }

        if (contractDetails == null) return false;

        option.MinTick = Convert.ToDecimal(contractDetails.MinTick);
        option.UnderlyingId = contractDetails.UnderConId;
        option.MarketRule = int.Parse(contractDetails.MarketRuleIds);

        option.ConId = contractDetails.Contract.ConId;
        option.LocalSymbol = contractDetails.Contract.LocalSymbol;
        option.Symbol = contractDetails.Contract.Symbol;
        option.Echange = contractDetails.Contract.Exchange;
        option.Currency = contractDetails.Contract.Currency;
        option.Multiplier = int.Parse(contractDetails.Contract.Multiplier);
        option.LastTradeDate = DateTime.ParseExact(contractDetails.Contract.LastTradeDateOrContractMonth, "yyyyMMdd", CultureInfo.CurrentCulture);
        option.InstumentType = InstumentType.Option;
        option.OptionType = contractDetails.Contract.Right == "C" ? OptionType.Call : OptionType.Put;
        option.Strike = (Decimal)contractDetails.Contract.Strike;
        option.TradingClass = contractDetails.Contract.TradingClass;

        //ClientSocket.reqMktData(contractDetails.Contract.ConId, contractDetails.Contract, string.Empty, false, false, null);
        //ClientSocket.reqMarketRule(int.Parse(contractDetails.MarketRuleIds));

        return true;
    }

    public override void contractDetails(int reqId, ContractDetails contractDetails)
    {
        if (contractDetails.Contract.SecType == "FUT")
        {
            lock (_futureLock)
            {
                _futureQueue.Enqueue((reqId, contractDetails));
                Monitor.Pulse(_futureLock);
            }
        }

        if (contractDetails.Contract.SecType == "FOP")
        {
            lock (_optionLock)
            {
                _optionQueue.Enqueue((reqId, contractDetails));
                Monitor.PulseAll(_optionLock);
            }

            return;
        }
    }
    public override void securityDefinitionOptionParameter(int reqId, string exchange, int underlyingConId, string tradingClass, string multiplier, HashSet<string> expirations, HashSet<double> strikes)
    {
        var fut = CachedFutures.Where(f => f.ConId == reqId).FirstOrDefault();
        if (fut != null)
        {
            foreach (var expiration in expirations)
            {
                fut.OptionChain.Add(new OptionChain() { 
                    TradingClass = tradingClass, 
                    ExpirationDate =  DateTime.ParseExact(expiration, "yyyyMMdd", CultureInfo.CurrentCulture), 
                    Strikes = strikes });
            }
        }
    }

    #region Add and Remove Instrument from Cache

    /// <summary>
    /// Добавляет в кешированые инструменты новый инструмент и запрашивает рыночные данные по нему.
    /// </summary>
    /// <param name="option">Собственно тот инструмент который будет добавлен и для кого будут запрошены данные</param>
    public void CacheOption(IOption option)
    {
        if (option.ConId != default && !CachedOptions.Any(co => co.ConId == option.ConId))
        {
            lock (_optionCollectionLock)
            {
                CachedOptions.Add(option);
            }
        }
        option.SetConnector(this);
        ClientSocket.reqMktData(option.ConId, option.ToIbContract(), string.Empty, false, false, null);
        ClientSocket.reqMarketRule(option.MarketRule);
    }
    public bool RemoveCachedFuture(IFuture future)
    {
        if (CachedFutures.Contains(future))
        {
            return CachedFutures.Remove(future);
        }
        return false;
    }
    public void CacheFuture(IFuture future)
    {
        if (future.ConId != default && !CachedFutures.Any(cf => cf.ConId == future.ConId))
        {
            CachedFutures.Add(future);
            ClientSocket.reqSecDefOptParams(future.ConId, future.Symbol, future.Echange, "FUT", future.ConId);
            ClientSocket.reqMktData(future.ConId, future.ToIbContract(), string.Empty, false, false, null);
        }
    }
    public bool RemoveCachedOption(IOption option)
    {
        if (CachedOptions.Contains(option))
        {
            return CachedOptions.Remove(option);
        }
        return false;
    }
    #endregion

    #endregion

    #region Orders 

    public void SendOptionOrder(IOrder order, IOption option)
    {
        if (order.OrderId == -1)
        {
            order.OrderId = nextOrderId++;
            OpenOrders.Add(order);
        }

        ClientSocket.placeOrder(order.OrderId, option.ToIbContract(), order.ToIbOrder());
    }
    public void CancelOrder(int OrderId)
    {
        ClientSocket.cancelOrder(OrderId);
    }
    public override void orderStatus(int orderId, string status, double filled, double remaining, double avgFillPrice, int permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice)
    {
        var openorder = OpenOrders.Where(o => o.OrderId == orderId).FirstOrDefault();
        if (openorder != null)
        {
            openorder.Status = status;
            openorder.AvgFilledPrice = Convert.ToDecimal(avgFillPrice);
            openorder.FilledQuantity = (int)filled;

            if (openorder.Status == "Submitted")
            {
                openorder.Submitted();
            }
        }
    }
    public override void openOrder(int orderId, Contract contract, Order order, OrderState orderState)
    {
        if (orderState.Commission != double.MaxValue)
        {
            var openorder = OpenOrders.Where(o => o.OrderId == orderId).FirstOrDefault();
            if (openorder != null)
            {
                if (openorder.FilledQuantity == openorder.TotalQuantity)
                {
                    openorder.Commission = Convert.ToDecimal(orderState.Commission);
                    openorder.Filled();
                    OpenOrders.Remove(openorder);
                }
            }
        }
    }
    
    #endregion

    #region MarketData

    public override void tickPrice(int tickerId, int field, double price, TickAttrib attribs)
    {
        var type = GetTickTypeByField(field);
        if (!type.HasValue) return;

        foreach (var future in CachedFutures)
        {
            if (future.ConId == tickerId && type.HasValue)
            {
                future.Notify(type.Value, price);
                return;
            }
        }

        foreach (var option in CachedOptions)
        {
            if (option.ConId == tickerId && type.HasValue)
            {
                option.Notify(type.Value, price);
                return;
            }
        }
    }
    public override void tickOptionComputation(int tickerId, int field, double impliedVolatility, double delta, double optPrice, double pvDividend, double gamma, double vega, double theta, double undPrice)
    {
        if (field != IBApi.TickType.MODEL_OPTION &&
            field != IBApi.TickType.DELAYED_MODEL_OPTION) 
            return;
        lock (_optionCollectionLock)
        {
            foreach (var option in CachedOptions)
            {
                if (option.ConId == tickerId)
                {
                    option.Notify(Connectors.Enums.TickType.TheorPrice, optPrice);
                }
            }
        }
    }
    private Connectors.Enums.TickType? GetTickTypeByField(int field) => field switch
    {
        IBApi.TickType.ASK => Connectors.Enums.TickType.Ask,
        IBApi.TickType.DELAYED_ASK => Connectors.Enums.TickType.Ask,
        IBApi.TickType.BID => Connectors.Enums.TickType.Bid,
        IBApi.TickType.DELAYED_BID => Connectors.Enums.TickType.Bid,
        IBApi.TickType.LAST => Connectors.Enums.TickType.LastPrice,
        IBApi.TickType.DELAYED_LAST => Connectors.Enums.TickType.LastPrice,
        _ => null
    };

    #endregion

    #region MarketRule
    public override void marketRule(int marketRuleId, PriceIncrement[] priceIncrements)
    {
        lock (_optionCollectionLock)
        {
            foreach (var option in CachedOptions)
            {
                if (option.MarketRule == marketRuleId)
                {
                    option.MinTick = (decimal)priceIncrements.Max(pi => pi.Increment);
                    continue;
                }
            }
        }
        base.marketRule(marketRuleId, priceIncrements);
    }

    #endregion

    #region Errors
    public override void error(int id, int errorCode, string errorMsg)
    {
       
        switch (errorCode)
        {
            case 10167:
            case 10197:

                break;
            case 110: // wrong order price
                if (OpenOrders.FirstOrDefault(o => o.OrderId == id) is IOrder orderwithwrongprice)
                {

                    OpenOrders.Remove(orderwithwrongprice);
                    orderwithwrongprice.Canceled("Wrong price");
                    _logger.LogError($"{DateTime.Now} Order with {id} have wrong price. Cancel it");
                }
                break;
            case 140:
                if (OpenOrders.FirstOrDefault(o => o.OrderId == id) is IOrder order)
                {
                    OpenOrders.Remove(order);
                    order.Canceled("Wrong quantity");
                }
                break;
            case 200:
                ContractDetails? contractDetails = null;
                lock(_futureLock)
                {
                    var fut_tup = (id, contractDetails);
                    _futureQueue.Enqueue(fut_tup);
                    Monitor.Pulse(_futureLock);
                }
                lock (_optionLock)
                {
                    var opt_tup = (id, contractDetails);
                    _optionQueue.Enqueue(opt_tup);
                    Monitor.Pulse(_optionLock);
                }
                break;
            case 202:
                if (OpenOrders.FirstOrDefault(o => o.OrderId == id) is IOrder canceledorder)
                {
                    _logger.LogError($"Error: {id}, {errorCode}, {errorMsg}");
                    OpenOrders.Remove(canceledorder);
                    canceledorder.Canceled("Manual");
                }
                break;
            default:
                _logger.LogError($"Error: {id}, {errorCode}, {errorMsg}");
                break;
        }
    }

    public override void error(string str)
    {
        _logger.LogError($"Error: {str}");
    }

    public override void error(Exception e)
    {
        _logger.LogError(e.Message);
        throw e;
    }
    #endregion
}
