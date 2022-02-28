using DataLayer.Enums;
using DataLayer.Interfaces;
using DataLayer.Models.Strategies;
using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public class LongStraddle : IEntity
{
    #region Data base references
    public int Id { get; set; }

    public List<OptionStrategy> OptionStrategies { get; set; } = new();

    #endregion

    public DateTime ExpirationDate { get; set; }
    public double Strike { get; set; }
    public StrategyLogic LongStraddleLogic { get; set; }
    public void Start()
    { }
}