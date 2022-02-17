namespace Connectors.Models.Instruments
{
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
