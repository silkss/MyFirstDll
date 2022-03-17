using Connectors.Enums;
using Connectors.Helpers;
using Connectors.Interfaces;
using DataLayer.Interfaces;
using DataLayer.Models.Strategies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models.Instruments;

public class DbOption : IOption, IEntity
{
    #region Props

    #region PublicProps

    #region DbReferences
    public int Id { get; set; }

    public List<OptionStrategy> OptionStrategies { get; set; } = new();
    #endregion

    public int ConId { get; set; }
    public decimal Strike { get; set; }
    public string TradingClass { get; set; }
    public OptionType OptionType { get; set; }
    public int UnderlyingId { get; set; }
    public string LocalSymbol { get; set; }
    public string Symbol { get; set; }
    public string Echange { get; set; }
    public string Currency { get; set; }
    public decimal MinTick { get; set; }
    public int Multiplier { get; set; }
    public int MarketRule { get; set; }
    public DateTime LastTradeDate { get; set; }
    public InstumentType InstumentType { get; set; }

    [NotMapped]
    public decimal Bid { get; set; }
    [NotMapped]
    public decimal Ask { get; set; }
    [NotMapped]
    public decimal LastPrice { get; set; }
    [NotMapped]
    public decimal TheorPrice { get; set; }

    public event Action<TickType> InstrumentChanged = delegate { };
    #endregion

    #region _privateProps
    private IConnector? _connector;
    #endregion

    #endregion

    #region Methods

    #region PublicMethods
    public void Notify(TickType type, double price)
    {
        switch (type)
        {
            case TickType.Bid:
                Bid = (decimal)price;
                break;
            case TickType.Ask:
                Ask = (decimal)price;
                break;
            case TickType.LastPrice:
                LastPrice = (decimal)price;
                break;
            case TickType.TheorPrice:
                TheorPrice = MathHelper.RoundUp((decimal)price, MinTick);
                break;
        }
    }

    public bool SendOrder(IOrder order, IOrderHolder orderHolder)
    {
        if (_connector == null) return false;
        if (TheorPrice <= 0) return false;

        order.OrderId = -1;
        order.LmtPrice = TheorPrice;
        order.SetOrderHolder(orderHolder);

        _connector.SendOptionOrder(order, this);
        return true; 
    }

    public void SetConnector(IConnector connector)
    {
        _connector = connector;
    }
    #endregion

    #endregion 
}
