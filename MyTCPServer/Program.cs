using Connectors.IB;
using Connectors.IB.Enums;
using Connectors.Interfaces;
using TCPGotm.Infrastructure;

namespace TCPGotm;

class Program
{
    static void Main(string[] args)
    {
        ILogger logger = new ConsoleLogger();
        IConnector connector = new IBConnector("127.0.0.1", 7497, 0, IbDataTypes.Delayed, logger);
        FabricOfTraders fabricOfTraders = new FabricOfTraders(connector, logger);

        TcpServer server = new TcpServer(fabricOfTraders, logger);

        server.Start();
    }
}
