using Tinkoff.Trading.OpenApi.Models;

namespace TinkoffPortfolioAnalyzer.Models
{
    internal class SecurityInfo
    {
        public string Ticker { get; set; }
        public InstrumentType InstrumentType { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
    }
}
