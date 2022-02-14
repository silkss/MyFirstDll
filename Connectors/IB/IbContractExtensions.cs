using Connectors.Models.Instruments;
using IBApi;
using System.Globalization;

namespace Connectors.Extensions
{
    public static class IbContractExtensions
    {
        public static Future ToFuture(this Contract contract) => new Future()
        {
            Id = contract.ConId,
            LocalSymbol = contract.LocalSymbol,
            Symbol = contract.Symbol,
            Echange = contract.Exchange,
            Currency = contract.Currency,
            Multiplier = int.Parse(contract.Multiplier),
            LastTradeDate = DateTime.ParseExact(contract.LastTradeDateOrContractMonth, "yyyyMMdd", CultureInfo.CurrentCulture),
            InstumentType = Enums.InstumentType.Future
        };

        public static Option ToOption(this Contract contract) => new Option()
        {
            Id = contract.ConId,
            LocalSymbol = contract.LocalSymbol,
            Symbol = contract.Symbol,
            Echange = contract.Exchange,
            Currency = contract.Currency,
            Multiplier = int.Parse(contract.Multiplier),
            LastTradeDate = DateTime.ParseExact(contract.LastTradeDateOrContractMonth, "yyyyMMdd", CultureInfo.CurrentCulture),
            InstumentType = Enums.InstumentType.Option,
            OptionType = contract.Right == "C" ? Enums.OptionType.Call : Enums.OptionType.Put,
            Strike = (Decimal)contract.Strike,
            TradingClass = contract.TradingClass
        };
    }
}
