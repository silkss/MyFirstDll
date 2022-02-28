using Connectors.Enums;
using Connectors.Extensions;
using Connectors.IB.Enums;
using Connectors.Interfaces;
using Connectors.Orders;
using IBApi;
using System.Globalization;
using Connectors.Utils;
using Connectors.Models.Instruments;

namespace Connectors.IB;

public class IBConnector<TFuture, TOption> : DefaultEWrapper, IConnector<TFuture, TOption>
    where TFuture : IFuture, new()
    where TOption : IOption, new()
{
    private string ip;
    private int port;
    private int clientid;
    private int nextOrderId;

    private IbDataTypes mktDataType;

    private readonly ILogger logger;
    private readonly EReaderSignal Signal;
    private readonly EClientSocket ClientSocket;

    private readonly object _futureLock = new();
    private readonly object _optionLock = new();
    private readonly Queue<(int, TFuture?)> _futureQueue = new();
    private readonly Queue<(int, TOption?)> _optionQueue = new();

    public Action<int, TFuture> FutureAdded { get; set; } = delegate { };
    public Action<int, TOption> OptionAdded { get; set; } = delegate { };

    public bool IsConnected
    {
        get { return ClientSocket.IsConnected(); }
    }

    public IBConnector()
    {
        var config = new IBConfig();
        ip = config.Ip;
        port = config.Port;
        clientid = config.ClientId;
        mktDataType = config.DataType;

        logger = new DebugLogger();

        Signal = new EReaderMonitorSignal();
        ClientSocket = new EClientSocket(this, Signal);

        OpenOrders = new List<GotOrder>();
        AccountList = new List<string>();

        logger.AddLog(LogType.Info, "Connector created");
    }

    public void Connect()
    {
        logger.AddLog(LogType.Info, "Connecting...");

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
        logger.AddLog(LogType.Warm, "Disconnected");
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

    #region HistoricalData
    public override void historicalData(int reqId, Bar bar)
    {
        base.historicalData(reqId, bar);
    }
    public override void historicalDataUpdate(int reqId, Bar bar)
    {
        base.historicalDataUpdate(reqId, bar);
    }
    #endregion

    #region Instruments
    private readonly List<TFuture> CachedFutures = new();
    private readonly List<TOption> CachedOptions = new();
    private readonly List<string> AccountList;
    private readonly List<GotOrder> OpenOrders;
    private readonly List<int> instrumentreqlist = new();
    public IEnumerable<string> GetAccountList() => AccountList;
    public IEnumerable<TFuture> GetCachedFutures() => CachedFutures;
    public IEnumerable<TOption> GetCachedOptions() => CachedOptions;

    public int RequestFuture(string localSymbol)
    {
        var contract = new Contract()
        {
            LocalSymbol = localSymbol,
            SecType = "FUT",
            Exchange = "GLOBEX",
            Currency = "USD"
        };
        var orderid = nextOrderId++;
        ClientSocket.reqContractDetails(orderid, contract);
        return orderid;
    }
    public async Task<TFuture?> RequestFutureAsync(string localSymbol)
    {
        if (!ClientSocket.IsConnected())
            return default(TFuture);

        int req = RequestFuture(localSymbol);
        TFuture? future = default(TFuture);
        await Task.Run(() =>
        {
            try
            {
                lock (_optionLock)
                {
                    while (!_futureQueue.TryPeek(out var tuple) && tuple.Item1 != req)
                    {
                        Monitor.Wait(_futureLock);
                    }
                    (_, future) = _futureQueue.Dequeue();
                }
            }
            catch (InvalidOperationException)
            {
                future = default(TFuture);
            }
        });

        return future;
    }
    public int RequestOption(DateTime LastTradeDate, double Strike, OptionType type, TFuture parent)
    {
        var contract = new Contract()
        {
            Symbol = parent.Symbol,
            SecType = "FOP",
            Exchange = parent.Echange,
            Currency = parent.Currency,
            LastTradeDateOrContractMonth = LastTradeDate.ToString("yyyyMMdd"),
            Strike = Strike,
            Right = type == OptionType.Call ? "C" : "P",
            Multiplier = parent.Multiplier.ToString()
        };
        var orderid = nextOrderId++;
        ClientSocket.reqContractDetails(orderid, contract);
        return orderid;
    }
    public async Task<TOption?> RequestOptionAsync(DateTime LastTradeDate, double strike, OptionType type, TFuture parent)
    {
        if (!ClientSocket.IsConnected())
            return default(TOption);

        int req = RequestOption(LastTradeDate, strike, type, parent);
        TOption? option = default(TOption);
        await Task.Run(() =>
        {
            try
            {
                lock (_optionQueue)
                {
                    while (!_optionQueue.TryPeek(out var tuple) && tuple.Item1 != req)
                    {
                        Monitor.Wait(_optionQueue);
                    }
                    (_, option) = _optionQueue.Dequeue();
                }
            }
            catch (InvalidOperationException)
            {
                option = default(TOption);
            }
        });

        return option;
    }
    public override void contractDetails(int reqId, ContractDetails contractDetails)
    {
        if (contractDetails.Contract.SecType == "FUT")
        {
            var future = contractDetails.Contract.ToFuture(new TFuture());
            future.MinTick = Convert.ToDecimal(contractDetails.MinTick);

            if (CachedFutures.FirstOrDefault(cf => cf.ConId == future.ConId) is TFuture alreadycached)
            {
                lock(_futureLock)
                {
                    _futureQueue.Enqueue((reqId, alreadycached));
                    Monitor.Pulse(_futureLock);
                }
                FutureAdded?.Invoke(reqId, alreadycached);
            }
            else
            {
                CacheFuture(future);
                lock (_futureLock)
                {
                    _futureQueue.Enqueue((reqId, future));
                    Monitor.Pulse(_futureLock);
                }
                FutureAdded?.Invoke(reqId, future);
            }
            return;
        }

        if (contractDetails.Contract.SecType == "FOP")
        {
            var option = contractDetails.Contract.ToOption(new TOption());
            option.MinTick = Convert.ToDecimal(contractDetails.MinTick);
            option.UnderlyingId = contractDetails.UnderConId;
            option.MarketRule = int.Parse(contractDetails.MarketRuleIds);

            if(CachedOptions.FirstOrDefault(co=> co.ConId == option.ConId) is TOption alreadycached)
            {
                lock (_optionLock)
                {
                    _optionQueue.Enqueue((reqId, option));
                    Monitor.Pulse(_optionLock);
                }
                OptionAdded?.Invoke(reqId, alreadycached);
            }
            else
            {
                CacheOption(option);
                lock (_optionLock)
                {
                    _optionQueue.Enqueue((reqId, option));
                    Monitor.Pulse(_optionLock);
                }
                OptionAdded?.Invoke(reqId, option);
                ClientSocket.reqMktData(option.ConId, option.ToIbContract(), string.Empty, false, false, null);
                ClientSocket.reqMarketRule(int.Parse(contractDetails.MarketRuleIds));
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
    public void CacheOption(TOption option)
    {
        if (option.ConId != default && !CachedOptions.Any(co => co.ConId == option.ConId))
        {
            CachedOptions.Add(option);
        }
    }
    public bool RemoveCachedFuture(TFuture future)
    {
        if (CachedFutures.Contains(future))
        {
            return CachedFutures.Remove(future);
        }
        return false;
    }
    public void CacheFuture(TFuture future)
    {
        if (future.ConId != default && !CachedFutures.Any(cf => cf.ConId == future.ConId))
        {
            CachedFutures.Add(future);
            ClientSocket.reqSecDefOptParams(future.ConId, future.Symbol, future.Echange, "FUT", future.ConId);
            ClientSocket.reqMktData(future.ConId, future.ToIbContract(), string.Empty, false, false, null);
        }
    }
    public bool RemoveCachedOption(TOption option)
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
    public void SendOptionOrder(GotOrder order, TOption option)
    {
        if (order.Id == -1)
        {
            order.Id = nextOrderId++;
        }

        logger.AddLog(LogType.Warm, $"Sending order with: price - {order.LmtPrice}");
        OpenOrders.Add(order);
        ClientSocket.placeOrder(order.Id, option.ToIbContract(), order.ToIbOrder());
    }
    public override void orderStatus(int orderId, string status, double filled, double remaining, double avgFillPrice, int permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice)
    {
        var openorder = OpenOrders.Where(o => o.Id == orderId).FirstOrDefault();
        if (openorder != null)
        {
            openorder.Status = status;
            openorder.AvgFilledPrice = Convert.ToDecimal(avgFillPrice);
            openorder.FilledQuantity = (int)filled;
        }
    }
    public override void openOrder(int orderId, Contract contract, Order order, OrderState orderState)
    {
        if (orderState.Commission != double.MaxValue)
        {
            var openorder = OpenOrders.Where(o => o.Id == orderId).FirstOrDefault();
            if (openorder != null)
            {
                if (openorder.FilledQuantity == openorder.TotalQuantity)
                {
                    openorder.Commission = Convert.ToDecimal(orderState.Commission);
                    openorder.Filled();
                    OpenOrders.Remove(openorder);
                }
                if (openorder.Status == "Submited")
                {
                    openorder.Submitted();
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

        foreach (var instrument in CachedFutures)
        {
            if (instrument.ConId == tickerId)
            {
                instrument.Notify(type.Value, price);
                return;
            }
        }

        foreach (var option in CachedOptions)
        {
            if (option.ConId == tickerId)
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

        foreach (var option in CachedOptions)
        {
            if (option.ConId == tickerId)
            {
                option.Notify(Connectors.Enums.TickType.TheorPrice, optPrice);
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
        foreach (var option in CachedOptions)
        {
            if (option.MarketRule == marketRuleId)
            {
                option.MinTick = (decimal)priceIncrements.Max(pi => pi.Increment);
            }
        }
        base.marketRule(marketRuleId, priceIncrements);
    }
    #endregion

    #region Errors
    public override void error(int id, int errorCode, string errorMsg)
    {
        logger.AddLog(LogType.Warm, $"Error: {id}, {errorCode}, {errorMsg}");
        switch (errorCode)
        {
            case 140:
                if (OpenOrders.FirstOrDefault(o => o.Id == id) is GotOrder order)
                {
                    OpenOrders.Remove(order);
                    order.Canceled();
                }
                break;
            case 200:
                lock(_futureLock)
                {
                    var fut_tup = (id, default(TFuture));
                    _futureQueue.Enqueue(fut_tup);
                    Monitor.Pulse(_futureLock);
                }
                lock (_optionLock)
                {
                    var opt_tup = (id, default(TOption));
                    _optionQueue.Enqueue(opt_tup);
                    Monitor.Pulse(_optionLock);
                }
                break;
            default:
                break;
        }
    }

    public override void error(string str)
    {
        logger.AddLog(LogType.Warm, $"Error: {str}");
    }

    public override void error(Exception e)
    {
        logger.AddLog(LogType.Crit, e.Message);
        throw e;
    }
    #endregion
}
