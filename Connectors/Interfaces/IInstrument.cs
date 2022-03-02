using Connectors.Enums;

namespace Connectors.Interfaces;

public interface IInstrument
{
    public int ConId { get; set; }
    public string LocalSymbol { get; set; }
    public string Symbol { get; set; }
    public string Echange { get; set; }
    public string Currency { get; set; }
    public decimal MinTick { get; set; }
    public int Multiplier { get; set; }
    public int MarketRule { get; set; }
    public DateTime LastTradeDate { get; set; }
    InstumentType InstumentType { get; set; }
    //public event Action<TickType> InstrumentChanged;
    public void Notify(TickType type, double price);

    decimal Bid { get; set; }
    decimal Ask { get; set; }
    decimal LastPrice { get; set; }
    decimal TheorPrice { get; set; }
}
