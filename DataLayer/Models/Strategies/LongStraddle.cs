using Connectors.Enums;
using Connectors.Interfaces;
using DataLayer.Enums;
using DataLayer.Interfaces;
using DataLayer.Models.Instruments;
using DataLayer.Models.Strategies;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Models;

public class LongStraddle : IEntity
{
    #region Props

    #region Data base references
    public int Id { get; set; }

    #region Container
    public int ContainerId { get; set; }
    public Container Container { get; set; }
    #endregion

    public List<OptionStrategy> OptionStrategies { get; set; } = new();

    #endregion

    #region PublicProps

    public DateTime ExpirationDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public double Strike { get; set; }

    #region Straddle Logic

    private IRepository<LongStraddle>? _straddleRepository;
    public StrategyLogic StraddleLogic { get; private set; }

    [NotMapped]
    public decimal PnLInCurrency => OptionStrategies.Sum(os => os.PnlInCurrency);
   

    #endregion

    #endregion

    #endregion

    #region Methods

    #region PublicMethods
    public void ChangeLogic(StrategyLogic strategyLogic)
    {
        StraddleLogic = strategyLogic;
        OptionStrategies.ForEach(s => s.StrategyLogic = StraddleLogic);
        if (_straddleRepository != null)
        {
            _straddleRepository.UpdateAsync(this);
        }
    }
    public void Start(IConnector connector, IRepository<LongStraddle> straddleRepository, IRepository<OptionStrategy> repository, IRepository<DbOrder> orderRepository)
    {
        _straddleRepository = straddleRepository;
        foreach (var optionstrategy in OptionStrategies)
        {
            optionstrategy.Start(connector, repository, orderRepository);
        }
    }
    public void Work(string account)
    {
        foreach (var strategy in OptionStrategies)
        {
            strategy.Work(account);
        }
    }
    public async Task<OptionStrategy> CreatAndAddStrategyAsync(DbOption option, int straddleId, IRepository<OptionStrategy> repository, int volume = 1)
    {
        var option_strategy = new OptionStrategy
        {
            Direction = Direction.Buy,
            OptionId = option.Id,
            LongStraddleId = straddleId,
            Volume = volume,
            StrategyLogic = StrategyLogic.OpenPoition
        };

        _ = await repository.CreateAsync(option_strategy);

        OptionStrategies.Add(option_strategy);
        option_strategy.Option = option;

        return option_strategy;
    }

    public void Stop()
    {
        foreach (var strategy in OptionStrategies)
        {
            strategy.Stop();
        }
    }

    #endregion

    #endregion
}