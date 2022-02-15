using Connectors.Enums;
using Connectors.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connectors.Models.Instruments.Base;

public class Instrument 
{
    public int Id { get; set; }
    public string LocalSymbol { get; init; }
    public string Symbol { get; init; }
    public string Echange { get; init; }
    public string Currency { get; init; }
    public decimal MinTick { get; set; }
    public int Multiplier { get; init; }
    public int MarketRule { get; set; }

    private DateTime _lastTradeDate;
    public DateTime LastTradeDate
    {
        get { return _lastTradeDate.Date; }
        set { _lastTradeDate = value; }
    }

    public virtual decimal GetTradablePrice() => LastPrice;

    public InstumentType InstumentType { get; init; }

    public event Action<Enums.TickType> InstrumentChanged = delegate { };
    public void Notify(Enums.TickType type, double price)
    {
        switch (type)
        {
            case Enums.TickType.Ask:
                Ask = (decimal)price;
                break;
            case Enums.TickType.Bid:
                Bid = (decimal)price;
                break;
            case Enums.TickType.LastPrice:
                LastPrice = (decimal)price;
                break;
            case Enums.TickType.TheorPrice:
                if (price != double.MaxValue)
                {
                    var theor_price = (decimal)price;
                    TheorPrice = MathHelper.RoundUp(theor_price, MinTick);
                }
                break;
            default: break;
        }
        InstrumentChanged?.Invoke(type);
    }

    #region Bid
    [NotMapped]
    public decimal Bid { get; set; }
    #endregion

    #region Ask
    [NotMapped]
    public decimal Ask { get; set; }
    #endregion

    #region Last Price
    [NotMapped]
    public decimal LastPrice { get; set; }
    #endregion

    #region Theor Price
    [NotMapped]
    public decimal TheorPrice { get; set; }
    #endregion
}