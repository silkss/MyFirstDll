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

    public void OnOrderFilled()
    {
        throw new NotImplementedException();
    }

    public void OnCanceled()
    {
        throw new NotImplementedException();
    }

    public void onFilledQunatityChanged()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region privateMethods
    private void openPositionLogic(string account)
    {
        if (Volume > Math.Abs(Position))
        {
            /*
             * Need To send order!
             */

            if (Option != null)
            {
                _openOrder = Option.SendOrder(Direction, account, Volume, this);
            }
        }
    }
    #endregion

    #endregion
}