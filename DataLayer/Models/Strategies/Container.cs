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

    public void OpenStraddle(double price)
    {
        var oc = Future.OptionChain
            .OrderByDescending(oc => oc.ExpirationDate)
            .Reverse()
            .FirstOrDefault(oc => _period > (oc.ExpirationDate - DateTime.Now));

        if (oc == null) return;

        var strike =oc.Strikes.OrderByDescending(s => s)
            .Reverse()
            .First(s => s > price);
        //TODO: придумать логику. И, возможно, сделать ее асинхронной.
    }

    public void CloseStraddle(double price)
    { }
}