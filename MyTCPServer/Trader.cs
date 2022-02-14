using Connectors.Models.Instruments;
using Connectors.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using TCPGotm.enums;
using TCPGotm.TradeLogic;

namespace TCPGotm;

internal class Trader
{
    private readonly TimeSpan _Period = new TimeSpan(days: 9,hours: 0,minutes: 0, seconds: 0);
    private readonly IConnector _Connector;
    private readonly ILogger _Logger;

    private readonly List<LongStraddle> Straddlers = new();

    public int reqUnderlyinId { get; set; } = 0;
    public Future? _Underlying { get; set; }

    public bool Inited { get; private set; } = false;

    public Trader(IConnector connector, ILogger logger)
    {
        _Logger = logger;
        _Connector = connector;
        _Connector.FutureAdded += onFutureAdded;
        if (!_Connector.IsConnected)
            _Connector.Connect();
    }

    private void onFutureAdded(int reqid, Future future)
    {
        if (reqid != reqUnderlyinId) return;
        _Underlying = future;
        Inited = true;
    }

    private void Init(string underlying)
    {
        reqUnderlyinId = _Connector.RequestFuture(underlying);
    }

    public void Trade(string underlying, decimal price, SignalType type)
    {
        switch (type)
        {
            case SignalType.INIT:
                Init(underlying);
                break;
            case SignalType.OPEN:
                try
                {
#if DEBUG
                    if (_Underlying == null) throw new NullReferenceException();
                    price = _Underlying.LastPrice;
#endif
                    var optioncChain = _Underlying.OptionChain
                        .OrderByDescending(oc => oc.ExpirationDate).Reverse()
                        .First();

                    var strike = optioncChain.Strikes
                        .OrderByDescending(s => s)
                        .First(s => (decimal)s < price);

                    if (Straddlers.Count() == 0)
                    {
                        Straddlers.Add(new LongStraddle(_Connector, _Underlying, strike, optioncChain.ExpirationDate));
                    }
                }
                catch (NullReferenceException)
                {
                    _Logger.AddLog(LogType.Crit, "Underlyer is NULL. Initing..");
                    Init(underlying);
                    return;
                }

                break;
            case SignalType.CLOSE:
                break;
            default:
                throw new System.ArgumentException("Wrong signal type!", nameof(type));
        }
    }

    private double nearestStrike(decimal price)
    {
        return default(double);
    }
}
