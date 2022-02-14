using Connectors.Enums;
using Connectors.Models.Instruments;
using Connectors.Orders;

namespace Connectors.Interfaces;

public interface IConnector
{
    Action<int, Future> FutureAdded { get; set; }
    Action<int, Option> OptionAdded { get; set; }

    bool IsConnected { get; }
    void Connect();
    void Disconnect();

    int RequestOption(DateTime LastTradeDate, double Strike, OptionType type, Future parent);
    int RequestFuture(string localSymbol);

    void CacheFuture(Future future);
    bool RemoveCachedFuture(Future future);
    void CacheOption(Option option);
    bool RemoveCachedOption(Option option);

    IEnumerable<Future> GetCachedFutures();
    IEnumerable<Option> GetCachedOptions();
    IEnumerable<string> GetAccountList();

    void SendOptionOrder(GotOrder order, Option instrument);
}