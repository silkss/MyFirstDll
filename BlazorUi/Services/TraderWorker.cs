using Connectors.Interfaces;
using DataLayer.Models.Strategies;

namespace BlazorUi.Services;

public class TraderWorker
{
    private readonly IConnector _connector;
    private readonly ILogger<TraderWorker> _logger;

    private readonly List<Container> _workingContainers;

    private TimeSpan _period = new(9, 0, 0, 0);



    public TraderWorker(IConnector connector, ILogger<TraderWorker> logger)
    {
        _connector = connector;
        _logger = logger;
    }

    public bool AddContainerToWork(Container container)
    {
        if (_workingContainers.Contains(container))
            return true;
        else
        {
            _workingContainers.Add(container);
            return true;
        }
    }

    public void SignalOnOpen(string symbol, double price)
    {

    }

    public void SignalOnClose(string symbol, double price)
    { 
    }
}
