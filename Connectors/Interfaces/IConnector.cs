using Connectors.Enums;
using Connectors.Orders;

namespace Connectors.Interfaces;

public interface IConnector<TFuture, TOption> 
    where TFuture : IFuture, new()
    where TOption : IOption, new()
{
    Action<int, TFuture> FutureAdded { get; set; }
    Action<int, TOption> OptionAdded { get; set; }

    bool IsConnected { get; }
    void Connect();
    void Disconnect();

    int RequestOption(DateTime LastTradeDate, double Strike, OptionType type, TFuture parent);
    int RequestFuture(string localSymbol);
    Task<TFuture?> RequestFutureAsync(string localSymbol);
    void CacheFuture(TFuture future);
    bool RemoveCachedFuture(TFuture future);
    void CacheOption(TOption option);
    bool RemoveCachedOption(TOption option);

    IEnumerable<TFuture> GetCachedFutures();
    IEnumerable<TOption> GetCachedOptions();
    IEnumerable<string> GetAccountList();

    void SendOptionOrder(GotOrder order, TOption option);
}