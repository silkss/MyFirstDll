using Connectors.Enums;
using Connectors.Models.Instruments;
using DataLayer.Enums;
using DataLayer.Models.Strategies.Base;
using System.Collections.Generic;

namespace DataLayer.Models.Strategies;

public class OptionStrategy : BaseStrategy
{
    #region DB
    #region Instrument
    public int InstrumentId { get; set; }
    public Option Option { get; set; }
    #endregion
    #region Straddle
    public LongStraddle LongStraddle { get; set; }
    public int LongStraddleId { get; set; }
    #endregion
    #region Orders
    public List<DbOrder> StrategyOrders { get; set; }
    #endregion
    #endregion

    public int Volume { get; set; }
    public Direction Direction { get; set; }
    public StrategyLogic StrategyLogic { get; set; }


}