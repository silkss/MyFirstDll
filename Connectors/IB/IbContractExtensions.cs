using Connectors.Interfaces;
using IBApi;
using System.Globalization;

namespace Connectors.Extensions
{
    public static class IbContractExtensions
    {
        //public static IFuture ToFuture<T>(this Contract contract)
        //{
        //    future.ConId = contract.ConId;
        //    future.LocalSymbol = contract.LocalSymbol;
        //    future.Symbol = contract.Symbol;
        //    future.Echange = contract.Exchange;
        //    future.Currency = contract.Currency;
        //    future.Multiplier = int.Parse(contract.Multiplier);
        //    future.LastTradeDate = DateTime.ParseExact(contract.LastTradeDateOrContractMonth, "yyyyMMdd", CultureInfo.CurrentCulture);
        //    future.InstumentType = Enums.InstumentType.Future;
        //    return future;
        //}

        //public static T ToOption<T>(this Contract contract, T option) where T : IOption
        //{
        //    option.ConId = contract.ConId;
        //    option.LocalSymbol = contract.LocalSymbol;
        //    option.Symbol = contract.Symbol;
        //    option.Echange = contract.Exchange;
        //    option.Currency = contract.Currency;
        //    option.Multiplier = int.Parse(contract.Multiplier);
        //    option.LastTradeDate = DateTime.ParseExact(contract.LastTradeDateOrContractMonth, "yyyyMMdd", CultureInfo.CurrentCulture);
        //    option.InstumentType = Enums.InstumentType.Option;
        //    option.OptionType = contract.Right == "C" ? Enums.OptionType.Call : Enums.OptionType.Put;
        //    option.Strike = (Decimal)contract.Strike;
        //    option.TradingClass = contract.TradingClass;
        //    return option;
        //}
    }
}
