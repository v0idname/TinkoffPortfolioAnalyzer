using Microsoft.EntityFrameworkCore;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Data
{
    public class PortfolioAnalyzerDb : DbContext
    {
        public DbSet<TinkoffToken> Tokens { get; set; }

        public DbSet<AvailSecSnapshot> Snapshots { get; set; }

        public PortfolioAnalyzerDb() { }

        public PortfolioAnalyzerDb(DbContextOptions<PortfolioAnalyzerDb> options) : base(options) { }
    }
}
