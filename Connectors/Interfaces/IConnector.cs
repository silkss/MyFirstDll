namespace Connectors.Interfaces;

public interface IConnector
{
    Action<int, IFuture> FutureAdded { get; set; }
    Action<int, IOption> OptionAdded { get; set; }

    bool IsConnected { get; }
    void Connect();
    void Connect(string ip, int port, int clientId);
    void Disconnect();

    //int RequestOption(DateTime LastTradeDate, double Strike, OptionType type, IFuture parent);
    int RequestFuture(string localSymbol, string exchange);
    bool TryRequestFuture(IFuture future, string exchange);
    bool TryRequestOption(IOption option, IFuture parent);
    void CacheFuture(IFuture future);
    bool RemoveCachedFuture(IFuture future);
    void CacheOption(IOption option);
    bool RemoveCachedOption(IOption option);

    void CancelOrder(int OrderId);

    IEnumerable<IFuture> GetCachedFutures();
    IEnumerable<IOption> GetCachedOptions();
    IEnumerable<string> GetAccountList();

    void SendOptionOrder(IOrder order, IOption option);
    IEnumerable<string> GetExchangeList();
}