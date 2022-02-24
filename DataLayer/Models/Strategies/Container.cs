using DataLayer.Interfaces;
using DataLayer.Models.Instruments;
using System.Collections.Generic;

namespace DataLayer.Models.Strategies;
/// <summary>
/// Хранит в себе Базовый инструмент и все стреддлы для него. 
/// закрыты, открытые, все!
/// </summary>
public class Container : IEntity
{
    public int Id { get; set; }
    #region  DbReference
    #region Future
    public int FutureId { get; set; }
    public DbFuture Future { get; set; }
    #endregion
    #endregion

    public string Account { get; set; }
    public List<LongStraddle> LongStraddles { get; set; } = new();
}