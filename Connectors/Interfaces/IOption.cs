using Connectors.Enums;
using IBApi;

namespace Connectors.Interfaces;

public interface IOption : IInstrument
{
    decimal Strike { get; set; }
    string TradingClass { get; set; }
    OptionType OptionType { get; set; }
    int UnderlyingId { get; set; }
    IOrder? SendOrder(Direction direction, string account, int quantity, IOrderHolder orderHolder);

    public Contract ToIbContract() => new Contract()
    {
        ConId = this.ConId,
        Currency = this.Currency,
        Exchange = this.Echange,
        Right = this.OptionType == OptionType.Call ? "C" : "P",
        Strike = Convert.ToDouble(this.Strike),
        TradingClass = this.TradingClass,
        Symbol = this.Symbol,
        LocalSymbol = this.LocalSymbol
    };
}
