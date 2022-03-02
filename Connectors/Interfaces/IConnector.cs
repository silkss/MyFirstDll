using Connectors.Enums;
using Connectors.Orders;

namespace Connectors.Interfaces;
/// <summary>
/// Заменить template elements на интерфейсы
/// </summary>
/// <typeparam name="TFuture"></typeparam>
/// <typeparam name="TOption"></typeparam>
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
    TOption? RequestOptionAsync(DateTime LastTradeDate, double strike, OptionType type, TFuture parent);
    void CacheFuture(TFuture future);
    bool RemoveCachedFuture(TFuture future);
    void CacheOption(TOption option);
    bool RemoveCachedOption(TOption option);

    IEnumerable<TFuture> GetCachedFutures();
    IEnumerable<TOption> GetCachedOptions();
    IEnumerable<string> GetAccountList();

    void SendOptionOrder(GotOrder order, TOption option);
}