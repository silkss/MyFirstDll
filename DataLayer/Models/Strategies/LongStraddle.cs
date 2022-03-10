using Connectors.Enums;
using Connectors.Interfaces;
using DataLayer.Enums;
using DataLayer.Interfaces;
using DataLayer.Models.Instruments;
using DataLayer.Models.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    public void Start(IConnector connector)
    {
        foreach (var optionstrategy in OptionStrategies)
        {
            optionstrategy.Start(connector);
        }
    }
    public void Work(string account)
    { 
        foreach (var strategy in OptionStrategies)
        {
            strategy.Work(account);
        }
    }
    public async Task<OptionStrategy> CreatAndAddStrategyAsync(DbOption option, IRepository<OptionStrategy> repository, int volume = 1)
    {
        var option_strategy = new OptionStrategy
        {
            Direction = Direction.Buy,
            OptionId = option.Id,
            Volume = volume,
            StrategyLogic = StrategyLogic.OpenPoition
        };

        _ = await repository.CreateAsync(option_strategy);

        OptionStrategies.Add(option_strategy);
        option_strategy.Option = option;

        return option_strategy;
    }
    public bool IsOpen() => !OptionStrategies
        .Any(os => os.StrategyLogic == StrategyLogic.ClosePostion);
    #endregion

    #endregion
}