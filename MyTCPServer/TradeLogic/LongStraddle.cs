using Connectors.Enums;
using Connectors.Models.Instruments;
using Connectors.Interfaces;
using Connectors.Models.Strategies;

using System;

using TCPGotm.TradableUnits.Logic;

namespace TCPGotm.TradeLogic;

internal class LongStraddle
{
    private readonly IConnector _Connector;
    private int _putOptionreqid = 0;
    private int _callOptionreqid = 0;

    public LogicType LogicType = LogicType.OpenPosition;
    public Future Undrelying { get; set; }
    public OptionStrategy? PutOptionStrategy { get; set; }
    public OptionStrategy? CallOptionStrategy { get; set; }

    public LongStraddle(IConnector connector, Future underlying, double strike, DateTime lasttradedate)
    {
        _Connector = connector;
        Undrelying = underlying;

        Undrelying.InstrumentChanged += onIstrumentChanged;
        _Connector.OptionAdded += onOptionAdded;

        _putOptionreqid = _Connector.RequestOption(lasttradedate, strike, OptionType.Put, Undrelying);
        _callOptionreqid = _Connector.RequestOption(lasttradedate, strike, OptionType.Call, Undrelying);
    }

    private void onOptionAdded(int reqId, Option option)
    {
        if (reqId == _putOptionreqid)
            PutOptionStrategy = new OptionStrategy { Instrument = option, InstrumentId = option.Id, Direction = Direction.Buy };

        if(reqId == _callOptionreqid)
            CallOptionStrategy = new OptionStrategy { Instrument = option, InstrumentId = option.Id, Direction = Direction.Buy };
    }
    private void onIstrumentChanged(TickType ticktype)
    {
        switch (ticktype)
        {
            case TickType.LastPrice when LogicType == LogicType.OpenPosition:
                if (CallOptionStrategy == null) break;
                CallOptionStrategy.OpenPosition(_Connector);

                if (PutOptionStrategy == null) break;
                PutOptionStrategy.OpenPosition(_Connector);
                break;
            case TickType.LastPrice when LogicType == LogicType.ClosePosition:
                break;

            default: break;
        }
    }

}
