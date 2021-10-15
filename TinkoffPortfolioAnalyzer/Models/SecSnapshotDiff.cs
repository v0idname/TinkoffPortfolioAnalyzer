namespace TinkoffPortfolioAnalyzer.Models
{
    public class SecSnapshotDiff
    {
        public string Ticker { get; set; }
        public string Name { get; set; }
        public bool IsSnap0Contains { get; set; }
        public bool IsSnap1Contains { get; set; }
    }
}
