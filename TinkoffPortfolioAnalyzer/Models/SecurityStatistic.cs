namespace TinkoffPortfolioAnalyzer.Models
{
    /// <summary>
    /// Класс, описывающий какую-либо одну характеристику ценной бумаги.
    /// </summary>
    internal class SecurityStatistic
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}
