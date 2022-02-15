using Connectors.Enums;
using Connectors.Helpers;
using Connectors.Models.Instruments.Base;
using IBApi;

namespace Connectors.Models.Instruments;

public class Option : Instrument
{
    public decimal Strike { get; set; }
    public string TradingClass { get; set; }
    public OptionType OptionType { get; set; }

    public int FutureId { get; set; }
    public Future Future { get; set; }

    public Contract ToIbContract() => new Contract()
    {
        ConId = this.Id,
        Currency = this.Currency,
        Exchange = this.Echange,
        Right = this.OptionType == OptionType.Call ? "C" : "P",
        Strike = Convert.ToDouble(this.Strike),
        TradingClass = this.TradingClass,
        Symbol = this.Symbol,
        LocalSymbol = this.LocalSymbol
    };

    public override decimal GetTradablePrice()
    {
        if (TheorPrice != 0m) return MathHelper.RoundUp(TheorPrice, MinTick);
        if (LastPrice != 0m) return LastPrice;
        return 0m;
    }
}