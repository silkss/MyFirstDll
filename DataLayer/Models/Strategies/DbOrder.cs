using Connectors.Enums;
using Connectors.Interfaces;
using DataLayer.Interfaces;
using System;

namespace DataLayer.Models.Strategies;

public class DbOrder : IOrder, IEntity
{
    #region Props

    #region PublicProps
    #region Db
    public int Id { get; set; }
    public int OptionStrategyId { get; set; }
    public OptionStrategy OptionStrategy { get; set; }

    #endregion

    public int OrderId { get; set; }
    public string? Account { get; set; }
    public decimal LmtPrice { get; set; }
    public Direction Direction { get; set; }
    public string OrderType { get; set; } = "LMT";
    public int TotalQuantity { get; set; }
    public int FilledQuantity { get; set; }
    public decimal Commission { get; set; }
    public decimal AvgFilledPrice { get; set; }
    public string? Status { get; set; }
    #endregion

    #region _privateProps
    private IOrderHolder? _orderHolder;
    #endregion
    #endregion

    #region Methods

    #region PublicMethods
    public void Canceled()
    {
        throw new NotImplementedException();
    }

    public void Filled()
    {
        throw new NotImplementedException();
    }
    public void Submitted()
    {
        throw new NotImplementedException();
    }

    public void SetOrderHolder(IOrderHolder orderHolder)
    {
        _orderHolder = orderHolder;
    }
    #endregion

    #endregion
}