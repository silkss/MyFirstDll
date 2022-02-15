using Connectors.Enums;
using IBApi;

namespace Connectors.Orders
{
    public class GotOrder
    { 
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

        public Order ToIbOrder() => new Order()
        {
            Account = Account,
            LmtPrice = Convert.ToDouble(LmtPrice),
            Action = Direction == Direction.Buy ? "BUY" : "SELL",
            OrderType = OrderType,
            TotalQuantity = TotalQuantity
        };

        private IOrderHolder? holder;
        public void Filled()
        {
            if (holder == null) return;
            holder.OnOrderFilled();
            holder = null;
        }
        public void Submitted()
        {
            if (holder == null) return;
            holder.onFilledQunatityChanged();
        }
        public void Canceled()
        {
            if (holder != null)
            {
                holder.OnCanceled();
            }
        }

        public GotOrder(IOrderHolder? holder)
        {
            this.holder = holder;
        }
        public GotOrder() : this(null)
        { }
    }
}
