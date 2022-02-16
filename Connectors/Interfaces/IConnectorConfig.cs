using Connectors.IB.Enums;

namespace Connectors.Interfaces;
public interface IConnectorConfig
{
    string Ip { get; set; }
    int Port { get; set; }
    int ClientId { get; set; }
    IbDataTypes DataType { get; set; }
}
