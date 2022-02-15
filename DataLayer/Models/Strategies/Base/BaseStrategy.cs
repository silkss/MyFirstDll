using DataLayer.Interfaces;

namespace DataLayer.Models.Strategies.Base;

public class BaseStrategy : IEntity
{
    public int Id { get; set; }
}