using Connectors.Enums;
using Connectors.Interfaces;
using DataLayer.Enums;
using DataLayer.Interfaces;
using DataLayer.Models.Instruments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public int FutureId { get; set; }
    public DbFuture Future { get; set; }

    #endregion

    public List<LongStraddle> LongStraddles { get; set; } = new();

    #endregion

    #region Public props

    [NotMapped]
    public bool Started { get; private set; }
    public string Account { get; set; }
    public decimal WantedPnl { get; set; }
    public int KeepAliveInDays { get; set; }
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
    public void Start(IConnector connector, IRepository<LongStraddle> straddleRepository, IRepository<OptionStrategy> strategyRepository, IRepository<DbOrder> orderRepository)
    {
        connector.CacheFuture(Future);
        foreach (var straddle in LongStraddles)
        {
            straddle.Start(connector, straddleRepository, strategyRepository, orderRepository);
        }
        Started = true;
        
        Future.Tick += onInstrumentChanged;
    }
    public void Stop()
    {
        Started = false;
        foreach (var straddle in LongStraddles)
        {
            straddle.Stop();
        }
        Future.Tick -= onInstrumentChanged;
    }
    /// <summary>
    /// вернет true если открытый стрэдл не набра необходимый ПиУ и время жизни не истекло.
    /// </summary>
    /// <returns></returns>
    public bool HasOpenStraddleWithPnl()
    {
        if (LongStraddles == null) return false;
        
        var open_straddle = LongStraddles.SingleOrDefault(ls => ls.StraddleLogic == StrategyLogic.OpenPoition);
        /* Если нет открытого стрэдлла, то надо создать новый */
        if (open_straddle == null) return false;

        /* Если стрэдл был создан больше чем Определное колво дней, нужен новый */
        if ((DateTime.Now - open_straddle.CreatedDate).TotalDays > KeepAliveInDays) return false;

        /* если открытый стрэдл накопил необходимый ПиУ, нужен нвоый! */
        if (open_straddle.PnLInCurrency > WantedPnl) return false;

        /* Во всех остальных случаях можно использовать существующий стрэддл! */
        return true;
    }
    public bool HasTodayOpenOrder()
    {
        var open_straddle = LongStraddles.SingleOrDefault(ls => ls.StraddleLogic == StrategyLogic.OpenPoition);
        if (open_straddle == null) return false;

        if (DateTime.Today.Day == open_straddle.CreatedDate.Date.Day) return true;
        return false;
    }
    public decimal GetAllPnl() => LongStraddles.Sum(l => l.PnLInCurrency);
    public decimal GetOpenPnl() => LongStraddles.Sum(l => l.StraddleLogic == StrategyLogic.OpenPoition ? l.PnLInCurrency : 0);

    public LongStraddle? HasStraddleInCollection(DateTime expirationdate, double price) =>
        LongStraddles?.Find(ls => ls.ExpirationDate == expirationdate && ls.Strike == price);
    #endregion

    #endregion
}