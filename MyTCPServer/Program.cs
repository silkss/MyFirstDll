using Connectors.IB;
using Connectors.IB.Enums;
using Connectors.Interfaces;

namespace TCPGotm;

class Program
{
    static void Main(string[] args)
    {
        ILogger logger = new ConsoleLogger();
        IConnector connector = new IBConnector("127.0.0.1", 7497, 0, IbDataTypes.Delayed, logger);
        TcpServer server = new TcpServer(connector, logger);

        server.Start();
    }
}
