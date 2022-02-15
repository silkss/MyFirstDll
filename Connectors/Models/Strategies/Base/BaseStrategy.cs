using Connectors.Enums;
using Connectors.Models.Instruments.Base;
using Connectors.Orders;

namespace Connectors.Models.Strategies.Base;

public class BaseStrategy<T> : Notifier, IOrderHolder where T : Instrument
{
    public int Id { get; set; }
    public int InstrumentId { get; set; }
    public T? Instrument { get; set; }
    public int TradableVolume => Volume - Math.Abs(Position);

    #region Volume
    private int _volume = 1;
    public int Volume
    {
        get => _volume;
        set => Set(ref _volume, value);
    }
    #endregion

    #region Position 
    public int Position => StrategyOrders.Sum(o => o.FilledQuantity) + (OpenOrder == null ? 0 : OpenOrder.FilledQuantity);
    #endregion

    #region Direction
    private Direction _direction;
    public Direction Direction
    {
        get => _direction;
        set => Set(ref _direction, value);
    }

    #endregion

    protected GotOrder? OpenOrder { get; set; }
    public List<GotOrder> StrategyOrders { get; } = new();

    #region IOrderHolder
    public void OnOrderFilled()
    {
        if (OpenOrder == null) return;
        StrategyOrders.Add(OpenOrder);
        OpenOrder = null;
    }
    public void OnCanceled()
    {
        if (OpenOrder == null) return;
        StrategyOrders.Add(OpenOrder);
        OpenOrder = null;
    }
    public void onFilledQunatityChanged()
    {
        throw new NotImplementedException();
    }
    #endregion
}