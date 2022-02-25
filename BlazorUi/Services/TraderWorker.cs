using Connectors.Interfaces;
using DataLayer.Models;
using DataLayer.Models.Instruments;
using DataLayer.Models.Strategies;

namespace BlazorUi.Services;

public class TraderWorker
{
    private readonly IConnector<DbFuture, DbOption> _connector;

    public TraderWorker(IConnector<DbFuture, DbOption> connector)
    {
        _connector = connector;
    }
    public List<Container> WorkingContainers { get; } = new();
    public void StartContainer(Container item)
    {
        if (WorkingContainers.Contains(item) || item.Started)
        {
            return;
        }
        item.Start();
        WorkingContainers.Add(item);
    }

    public void StopContainer(Container item)
    {
        if (!WorkingContainers.Contains(item))
        {
            item.Stop();
            return;
        }
        item.Stop();
        WorkingContainers.Remove(item);
    }

    public void SignalOnOpen(string symbol, double price)
    {
        if (getContainer(symbol) is Container container)
        {
            var straddle = container.ChooseBestOptionChain(price);
            if (container.HasStraddleInCollection(straddle) is LongStraddle collectionStraddle)
            {
                collectionStraddle.Start();
            }
            else
            {
                
            }
        }
    }

    public void SignalOnClose(string symbol, double price)
    { 
    }

    private Container? getContainer(string symbol) => WorkingContainers.FirstOrDefault(c => c.Future.Symbol == symbol);
}
