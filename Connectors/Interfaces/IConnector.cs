using Connectors.Enums;
using Connectors.Orders;

namespace Connectors.Interfaces;
/// <summary>
/// Заменить template elements на интерфейсы
/// </summary>
/// <typeparam name="TFuture"></typeparam>
/// <typeparam name="TOption"></typeparam>
public interface IConnector
{
    Action<int, IFuture> FutureAdded { get; set; }
    Action<int, IOption> OptionAdded { get; set; }

    bool IsConnected { get; }
    void Connect();
    void Disconnect();

    //int RequestOption(DateTime LastTradeDate, double Strike, OptionType type, IFuture parent);
    int RequestFuture(string localSymbol);
    bool TryRequestFuture(IFuture future);
    bool TryRequestOption(IOption option, IFuture parent);
    void CacheFuture(IFuture future);
    bool RemoveCachedFuture(IFuture future);
    void CacheOption(IOption option);
    bool RemoveCachedOption(IOption option);

    IEnumerable<IFuture> GetCachedFutures();
    IEnumerable<IOption> GetCachedOptions();
    IEnumerable<string> GetAccountList();

    void SendOptionOrder(IOrder order, IOption option);
}