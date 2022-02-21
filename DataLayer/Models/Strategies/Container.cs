using DataLayer.Interfaces;
using DataLayer.Models.Instruments;
using System.Collections.Generic;

namespace DataLayer.Models;

public class Container : IEntity
{
    public int Id { get; set; }
    public int FutureId { get; set; }
    public DbFuture Future { get; set; }
    public List<LongStraddle> LongStraddles { get; set; } = new();
}