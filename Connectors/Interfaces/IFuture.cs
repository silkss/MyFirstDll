using Connectors.Enums;
using Connectors.Models.Instruments;

namespace Connectors.Interfaces;

public interface IFuture : IInstrument
{
    event Action<TickType, double> Tick;
    List<OptionChain> OptionChain { get; }
    IBApi.Contract ToIbContract() => new IBApi.Contract()
    {
        ConId = this.ConId,
        LocalSymbol = this.LocalSymbol,
        Currency = this.Currency,
        Exchange = this.Echange,
        SecType = "FUT",
        Symbol = this.Symbol
    };
}
