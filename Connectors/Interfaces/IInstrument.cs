using Connectors.Enums;

namespace Connectors.Interfaces;

public interface IInstrument
{
    #region Props

    int ConId { get; set; }
    string LocalSymbol { get; set; }
    string Symbol { get; set; }
    string Echange { get; set; }
    string Currency { get; set; }
    decimal MinTick { get; set; }
    int Multiplier { get; set; }
    int MarketRule { get; set; }
    DateTime LastTradeDate { get; set; }
    InstumentType InstumentType { get; set; }
    //public event Action<TickType> InstrumentChanged;
    void Notify(TickType type, double price);

    decimal Bid { get; set; }
    decimal Ask { get; set; }
    decimal LastPrice { get; set; }
    decimal TheorPrice { get; set; }
    #endregion

    #region Methods
    void SetConnector(IConnector connector);
    #endregion
}
