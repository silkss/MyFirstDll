using Connectors.IB.Enums;
using Connectors.Interfaces;

namespace Connectors.IB;

public class IBConfig : IConnectorConfig
{
    public string Ip { get; set; }
    public int Port { get; set; }
    public int ClientId { get; set; }
    public IbDataTypes DataType { get; set; }

    public IBConfig()
    {
        Ip = "127.0.0.1";
        Port = 7497;
        ClientId = 1;
        DataType = IbDataTypes.Delayed;
    }
}
