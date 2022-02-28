using Connectors.Enums;
using Connectors.Interfaces;
using Connectors.Models.Instruments;
using DataLayer.Interfaces;
using DataLayer.Models.Instruments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DataLayer.Models.Strategies;

/// <summary>
/// Хранит в себе Базовый инструмент и все стреддлы для него. 
/// закрыты, открытые, все!
/// </summary>
public class Container : IEntity
{
    private TimeSpan _period = new(9, 0, 0, 0);
    public int Id { get; set; }

    #region  DbReference
    #region Future
    public int FutureId { get; set; }
    public DbFuture Future { get; set; }
    #endregion

    public List<LongStraddle> LongStraddles { get; set; } = new();

    #endregion

    [NotMapped]
    public bool Started { get; private set; }
    public string Account { get; set; }

    public void Start()
    {
        Started = true;
    }

    public void Stop()
    {
        Started = false;
    }

    public LongStraddle? ChooseBestOptionChain(double price)
    {
        var optionChain = Future.OptionChain
            .OrderByDescending(oc => oc.ExpirationDate)
            .Reverse()
            .FirstOrDefault(oc => _period > (oc.ExpirationDate - DateTime.Now));

        if (optionChain == null) return null;

        var strike = optionChain.Strikes.OrderByDescending(s => s)
            .Reverse()
            .First(s => s > price);

        return new LongStraddle { ExpirationDate = optionChain.ExpirationDate, Strike = strike };
    }
    public LongStraddle? HasStraddleInCollection(LongStraddle straddle) =>
        LongStraddles.Find(ls => ls.ExpirationDate == straddle.ExpirationDate && ls.Strike == straddle.Strike);
    public void CloseStraddle(double price)
    { }
}