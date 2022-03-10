using Connectors.Enums;
using Connectors.Interfaces;
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
    public int Id { get; set; }

    #region Future
    public int? FutureId { get; set; }
    public DbFuture Future { get; set; }
    #endregion

    public List<LongStraddle> LongStraddles { get; set; } = new();
    #endregion

    #region Public props

    [NotMapped]
    public bool Started { get; private set; }
    public string Account { get; set; }
    public DateTime LastTradeDate { get; set; }
    #endregion

    #endregion

    #region Methods

    #region Private Methods
    private void onInstrumentChanged(TickType type, double price)
    {
        foreach (var straddle in LongStraddles)
        {
            straddle.Work(Account);
        }
    }
    #endregion

    #region Public Methods
    public void AddStraddle(LongStraddle straddle)
    {
        straddle.ContainerId = Id;
        LongStraddles.Add(straddle);
    }

    public void Start(IConnector connector)
    {
        connector.CacheFuture(Future);
        foreach (var straddle in LongStraddles)
        {
            straddle.Start(connector);
        }
        Started = true;
        
        Future.Tick += onInstrumentChanged;
    }

    public void Stop()
    {
        Started = false;
        Future.Tick -= onInstrumentChanged;
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
    public LongStraddle? HasStraddleInCollection(DateTime expirationdate, double price) =>
        LongStraddles?.Find(ls => ls.ExpirationDate == expirationdate && ls.Strike == price);

    public void CloseStraddle(double price)
    { }
    #endregion

    #endregion
}