using Connectors.Enums;
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
    #region Props
    #region Private props
    private TimeSpan _period = new(9, 0, 0, 0);
    #endregion

    #region  DbReference

    #region Future
    public int? FutureId { get; set; }
    public DbFuture Future { get; set; }
    #endregion
    public int Id { get; set; }

    public List<LongStraddle>? LongStraddles { get; set; }
    #endregion

    #region Public props

    [NotMapped]
    public bool Started { get; private set; }
    public string Account { get; set; }
    #endregion
    #endregion

    #region Methods

    #region Private Methods
    private void onInstrumentChanged(TickType type, double price)
    {

    }
    #endregion

    #region Public Methods
    public void Start()
    {
        Started = true;
        Future.Tick += onInstrumentChanged;
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
        LongStraddles?.Find(ls => ls.ExpirationDate == straddle.ExpirationDate && ls.Strike == straddle.Strike);
    public void CloseStraddle(double price)
    { }
    #endregion

    #endregion
}