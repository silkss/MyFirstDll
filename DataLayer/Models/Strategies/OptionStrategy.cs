﻿using Connectors.Interfaces;
using DataLayer.Models.Instruments;
using DataLayer.Models.Strategies.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models.Strategies;

public class OptionStrategy : BaseStrategy
{
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

    #region Methods

    #region PublicMethods

    public void Start(IConnector connector)
    {
        if (Option != null)
            connector.CacheOption(Option);
    }

    public void Work()
    {
        switch (StrategyLogic)
        {
            case Enums.StrategyLogic.OpenPoition:
                openPositionLogic();
                break;
            case Enums.StrategyLogic.ClosePostion:
                break;
            case Enums.StrategyLogic.Done:
                break;
        }
    }

    private void openPositionLogic()
    {
        if (Volume < Math.Abs(Position))
        {
            /*
             * Need To send order!
             */

            if (Option != null)
            {
                Option.SendOrder(); 
            }
        }
    }

    #endregion

    #endregion
}