using Connectors.Enums;
using DataLayer.Enums;
using DataLayer.Interfaces;

namespace DataLayer.Models.Strategies.Base;

public class BaseStrategy : IEntity
{
    public int Id { get; set; }
    public int Volume { get; set; }
    public int Position { get; set; }
    public Direction Direction { get; set; }
    public StrategyLogic StrategyLogic { get; set; }
}