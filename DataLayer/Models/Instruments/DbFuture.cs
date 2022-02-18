using Connectors.Enums;
using Connectors.Interfaces;
using Connectors.Models.Instruments;
using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models.Instruments;

public class DbFuture : IFuture, IEntity
{
    [NotMapped]
    public List<OptionChain> OptionChain { get; } = new();

    public int Id { get; set; }
    public int ConId { get; set; }
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

    public void Notify(TickType type, double price)
    {
        //throw new NotImplementedException();
    }
}
