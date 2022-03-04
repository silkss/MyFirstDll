using Connectors.Enums;
using IBApi;

namespace Connectors.Interfaces;

public interface IOrder
{
    #region Props
    int OrderId { get; set; }
    string? Account { get; set; }
    decimal LmtPrice { get; set; }
    Direction Direction { get; set; }
    string OrderType { get; set; }
    int TotalQuantity { get; set; }
    int FilledQuantity { get; set; }
    decimal Commission { get; set; }
    decimal AvgFilledPrice { get; set; }
    decimal TotalFilledPrice => AvgFilledPrice * FilledQuantity;
    string? Status { get; set; }
    #endregion

    #region Method
    Order ToIbOrder() => new Order()
    {
        Account = Account,
        LmtPrice = Convert.ToDouble(LmtPrice),
        Action = Direction == Direction.Buy ? "BUY" : "SELL",
        OrderType = OrderType,
        TotalQuantity = TotalQuantity
    };
    void SetOrderHolder(IOrderHolder orderHolder);
    void Filled();
    void Submitted();
    void Canceled();
    #endregion
}
