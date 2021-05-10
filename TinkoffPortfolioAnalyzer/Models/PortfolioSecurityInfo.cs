namespace TinkoffPortfolioAnalyzer.Models
{
    internal class PortfolioSecurityInfo : SecurityInfo
    {
        public int Amount { get; set; }

        public decimal TotalPrice
        {
            get
            {
                return Price * Amount;
            }
        }
    }
}
