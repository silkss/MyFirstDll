using Connectors.Interfaces;

namespace TCPGotm.Infrastructure;
/// <summary>
/// Фабрика должна хранить все страты. И при запросе нового торговца, должна выдавать торговца с уже имеющейся стартегией.
/// Если нет такого торговца, то необходимо создать нового.
/// </summary>
internal class FabricOfTraders
{
    //private readonly IConnector _Connector;
    private readonly ILogger _Logger;

    public FabricOfTraders(IConnector connector, ILogger logger)
    {
        //_Connector = connector;
        _Logger = logger;
    }

    public Trader CreateTrader() => new Trader(_Connector, _Logger);
}