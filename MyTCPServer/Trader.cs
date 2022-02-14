using Connectors.Instruments;
using Connectors.Interfaces;
using System;
using TCPGotm.enums;

namespace TCPGotm;

internal class Trader
{
    private readonly IConnector _Connector;
    private readonly ILogger _Logger;

    private int reqUnderlyinId = 0;
    private Future? _Underlying;

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
                    nearestStrike(price);
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
        if (_Underlying == null)
            throw new NullReferenceException();
    }
}
