using Connectors.Models.Instruments.Base;
using IBApi;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connectors.Models.Instruments
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

    public class OptionChain
    {
        private string _tradingClass = string.Empty;
        public string TradingClass 
        { 
            get { return _tradingClass; }
            set { _tradingClass = value; }
        }
        public DateTime ExpirationDate { get; set; }

        public HashSet<double> Strikes = new();
    }
}
