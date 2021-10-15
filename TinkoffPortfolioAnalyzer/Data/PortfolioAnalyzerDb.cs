using Microsoft.EntityFrameworkCore;
using TinkoffPortfolioAnalyzer.Data.Entities;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Data
{
    public class PortfolioAnalyzerDb : DbContext
    {
        public DbSet<TinkoffToken> Tokens { get; set; }

        public DbSet<SecurityInfo> AvailSecurities { get; set; }

        public DbSet<AvailSecSnapshot> Snapshots { get; set; }

        public PortfolioAnalyzerDb(DbContextOptions<PortfolioAnalyzerDb> options) : base(options) { }
    }
}
