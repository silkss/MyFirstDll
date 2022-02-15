using Connectors.Interfaces;
using Connectors.Models.Instruments;
using Connectors.Models.Strategies.Base;
using Connectors.Orders;

namespace Connectors.Models.Strategies;

public class OptionStrategy : BaseStrategy<Option>
{
    public void OpenPosition(IConnector connector)
    {
        if (Instrument == null) return;
        if (TradableVolume == 0) return;
        if (OpenOrder == null)
        {
            OpenOrder = new GotOrder(this)
            {
                Id = -1,
                TotalQuantity = TradableVolume,
                LmtPrice = Instrument.GetTradablePrice(),
                Direction = Direction,
            };
            connector.SendOptionOrder(OpenOrder, Instrument);
        }
    }
}

