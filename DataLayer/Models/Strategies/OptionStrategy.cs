using Connectors.Interfaces;
using DataLayer.Models.Instruments;
using DataLayer.Models.Strategies.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models.Strategies;

public class OptionStrategy : BaseStrategy, IOrderHolder
{
    #region Props

    #region PublicProps
    #region DB
    #region Instrument
    public int? OptionId { get; set; }
    public DbOption Option { get; set; }
    #endregion
    #region Straddle
    public int? LongStraddleId { get; set; }

    public LongStraddle LongStraddle { get; set; }
    #endregion
    #region Orders
    [NotMapped]
    public List<DbOrder> StrategyOrders { get; set; }
    #endregion
    #endregion
    #endregion

    #region _privateProps
    private IOrder? _openOrder;
    #endregion

    #endregion

    #region Methods

    #region PublicMethods

    public void Start(IConnector connector)
    {
        if (Option != null)
            connector.CacheOption(Option);
    }

    public void Work(string account)
    {
        switch (StrategyLogic)
        {
            case Enums.StrategyLogic.OpenPoition when _openOrder == null:
                openPositionLogic(account);
                break;
            case Enums.StrategyLogic.ClosePostion:
                break;
            case Enums.StrategyLogic.Done:
                break;
        }
    }

    public void OnOrderFilled(int orderId)
    {
        if (_openOrder == null) return;
        if (_openOrder.OrderId != orderId) return;
        Position = _openOrder.FilledQuantity;
        _openOrder = null;
        /*
         * необходимо сохранять изменения стратегии и 
         * ордера.
         */
    }

    public void OnCanceled(int orderId)
    {
        if (_openOrder == null) return;
        if (_openOrder.OrderId != orderId) return;
        _openOrder = null;
    }

    public void onFilledQunatityChanged(int orderId)
    {
        //throw new NotImplementedException();
    }

    #endregion

    #region privateMethods
    private void openPositionLogic(string account)
    {
        if (Volume > Math.Abs(Position))
        {
            if (Option != null)
            {
                _openOrder = Option.SendOrder(Direction, account, Volume - Math.Abs(Position), this);
            }
        }
    }
    #endregion

    #endregion
}