namespace TinkoffPortfolioAnalyzer.Models
{
    public class PortfolioSecurityInfo : SecurityInfo
    {
        public decimal AveragePrice { get; set; }

        public int Amount { get; set; }

        public decimal TotalPrice
        {
            get
            {
                return AveragePrice * Amount;
            }
        }
    }
}
