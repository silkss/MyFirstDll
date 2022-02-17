using Connectors.Models.Instruments;
using Connectors.Models_.Instruments.Base;
using IBApi;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connectors.Models_.Instruments
{
    public class Future : Instrument
    {
        [NotMapped]
        public List<OptionChain> OptionChain { get; } = new();
        public List<Option> Options { get; } = new();

        public Contract ToIbContract() => new Contract()
        {
            ConId = this.Id,
            LocalSymbol = this.LocalSymbol,
            Currency = this.Currency,
            Exchange = this.Echange,
            SecType = "FUT",
            Symbol = this.Symbol
        };
    }
}
