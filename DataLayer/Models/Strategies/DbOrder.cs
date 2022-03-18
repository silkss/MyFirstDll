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
    public int? OptionStrategyId { get; set; }
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

    public DateTime GeneratedTime { get; set; }
    public DateTime ExecuteTime { get; set;  }
    #endregion

    #region _privateProps
    private IOrderHolder? _orderHolder;
    #endregion

    #endregion

    #region Methods

    #region PublicMethods
    public void Canceled()
    {
        if (_orderHolder != null)
        {
            _orderHolder.OnCanceled(OrderId);
        }
    }

    public void Filled()
    {
        if (_orderHolder != null)
        {
            _orderHolder.OnOrderFilled(OrderId);
        }
    }
    public void Submitted()
    {

    }

    public void SetOrderHolder(IOrderHolder orderHolder)
    {
        _orderHolder = orderHolder;
    }
    #endregion

    #endregion
}