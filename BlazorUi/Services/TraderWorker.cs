using Connectors.Enums;
using Connectors.Interfaces;
using DataLayer.Enums;
using DataLayer.Models;
using DataLayer.Models.Instruments;
using DataLayer.Models.Strategies;
using Microsoft.Extensions.Logging;

namespace BlazorUi.Services;

public class TraderWorker
{
    private readonly IConnector _connector;
    private readonly ILogger<TraderWorker> _logger;
    private TimeSpan _period = new(9, 0, 0, 0);

    public TraderWorker(IConnector connector, ILogger<TraderWorker> logger)
    {
        _connector = connector;
        _logger = logger;
    }

    public void SignalOnOpen(string symbol, double price)
    {

    }

    public void SignalOnClose(string symbol, double price)
    { 
    }
}
