﻿using DataLayer.Interfaces;
using DataLayer.Models.Strategies;
using System;
using System.Collections.Generic;

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

    public DateTime ExpirationDate { get; set; }
    public double Strike { get; set; }
    public void Work()
    { 
        foreach (var strategy in OptionStrategies)
        {
            strategy.Work();
        }
    }
}