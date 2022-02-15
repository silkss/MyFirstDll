using DataLayer.Interfaces;
using System.Collections.Generic;

namespace DataLayer.Models;

public class Container : IEntity
{
    public List<LongStraddle> LongStraddles { get; set; } = new();
    public int Id { get; set; }
}