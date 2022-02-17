using Connectors.Models.Instruments;
using IBApi;

namespace Connectors.Interfaces;

public interface IFuture : IInstrument
{
    List<OptionChain> OptionChain { get; }
    Contract ToIbContract() => new Contract()
    {
        ConId = this.Id,
        LocalSymbol = this.LocalSymbol,
        Currency = this.Currency,
        Exchange = this.Echange,
        SecType = "FUT",
        Symbol = this.Symbol
    };
}
