using DataLayer.Models;
using System.Collections.Generic;

namespace WebUi.Services;

public class StrategeisRepository
{
    private readonly List<Container> _containers = new List<Container>();

    public Container? Find(string instrumentSymbol) => _containers.Find(c => c.Future.LocalSymbol == instrumentSymbol);
}
