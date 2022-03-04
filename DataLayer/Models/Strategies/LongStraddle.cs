using Connectors.Enums;
using DataLayer.Enums;
using DataLayer.Interfaces;
using DataLayer.Models.Instruments;
using DataLayer.Models.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Models;

public class LongStraddle : IEntity
{
    #region Data base references
    public int Id { get; set; }

    #region Container
    public int? ContainerId { get; set; }
    public Container Container { get; set; }
    #endregion

    public List<OptionStrategy> OptionStrategies { get; set; } = new();

    #endregion

    #region Methods 

    #region PublicMethods
    public DateTime ExpirationDate { get; set; }
    public double Strike { get; set; }
    public void Work()
    { 
        foreach (var strategy in OptionStrategies)
        {
            strategy.Work();
        }
    }
    public OptionStrategy CreatAndAddStrategy(DbOption option, int volume = 1)
    {
        var option_strategy = new OptionStrategy
        {
            Direction = Direction.Buy,
            OptionId = option.Id,
            Option = option,
            Volume = volume,
            StrategyLogic = StrategyLogic.OpenPoition
        };

        OptionStrategies.Add(option_strategy);
        return option_strategy;
    }
    public bool IsOpen() => !OptionStrategies
        .Any(os => os.StrategyLogic == StrategyLogic.ClosePostion);
    #endregion

    #endregion
}