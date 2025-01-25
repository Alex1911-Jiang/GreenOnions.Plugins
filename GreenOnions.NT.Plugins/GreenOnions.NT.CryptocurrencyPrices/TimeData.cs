using Binance.Net.Enums;

namespace GreenOnions.NT.CryptocurrencyPrices
{
    public class TimeData
    {
        public long Second { get; set; }
        public string Text { get; set; }
        public PositionSide Side { get; set; }
        public bool ShowCommand { get; set; }
    }

}