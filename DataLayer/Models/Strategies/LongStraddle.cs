using DataLayer.Enums;
using DataLayer.Interfaces;
using DataLayer.Models.Strategies;

namespace DataLayer.Models;

public class LongStraddle : IEntity
{
    #region Data base references
    public int Id { get; set; }
    public int ContainerId { get; set; }
    public Container Container { get; set; }

    //public List<OptionStrategy> OptionStrategies { get; set; } = new();

    #endregion

    public StrategyLogic LongStraddleLogic { get; set; }

}