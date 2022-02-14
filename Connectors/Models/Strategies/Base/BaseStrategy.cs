using Connectors.Enums;
using Connectors.Models.Instruments.Base;

namespace Connectors.Models.Strategies.Base;

public class BaseStrategy<T> : Notifier where T: Instrument
{
    public int Id { get; set; }
    public int InstrumentId { get; set; }
    public T? Instrument { get; set; }

    #region Volume
    private int _volume;
    public int Volume
    {
        get => _volume;
        set => Set(ref _volume, value);
    }
    #endregion

    #region Position 
    private int _position;
    public int Position
    {
        get => _position;
        set => Set(ref _position, value);
    }
    #endregion

    #region Direction
    private Direction _direction;
    public Direction Direction
    {
        get => _direction;
        set => Set(ref _direction, value);
    }
    #endregion
}