using Connectors.Enums;
using IBApi;

namespace Connectors.Orders;

public abstract class GotOrder
{
    #region Props

    public int Id { get; set; }
    public string? Account { get; set; }
    public decimal LmtPrice { get; set; }
    public Direction Direction { get; set; }
    public string OrderType { get; set; } = "LMT";
    public int TotalQuantity { get; set; }
    public int FilledQuantity { get; set; }
    public decimal Commission { get; set; }
    public decimal AvgFilledPrice { get; set; }
    public decimal TotalFilledPrice => AvgFilledPrice * FilledQuantity;
    public string? Status { get; set; }

    #endregion

    public Order ToIbOrder() => new Order()
    {
        Account = Account,
        LmtPrice = Convert.ToDouble(LmtPrice),
        Action = Direction == Direction.Buy ? "BUY" : "SELL",
        OrderType = OrderType,
        TotalQuantity = TotalQuantity
    };

    public abstract void Filled();
    public abstract void Submitted();
    public abstract void Canceled();
}