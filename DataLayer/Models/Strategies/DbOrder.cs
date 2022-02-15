using Connectors.Orders;
using DataLayer.Interfaces;
using System;

namespace DataLayer.Models.Strategies;
public class DbOrder : GotOrder, IEntity
{
    #region Db
    public int OptionStrategyId { get; set; }
    public OptionStrategy OptionStrategy { get; set; }
    #endregion

    public override void Canceled()
    {
        throw new NotImplementedException();
    }

    public override void Filled()
    {
        throw new NotImplementedException();
    }

    public override void Submitted()
    {
        throw new NotImplementedException();
    }
}