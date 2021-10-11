using Microsoft.EntityFrameworkCore;

namespace TinkoffPortfolioAnalyzer.Data
{
    class PortfolioAnalyzerDb : DbContext
    {
        public DbSet<TokenEntity> Tokens { get; set; }

        public PortfolioAnalyzerDb(DbContextOptions<PortfolioAnalyzerDb> options) : base(options) { }
    }
}
