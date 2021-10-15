using Microsoft.EntityFrameworkCore;
using System;
using TinkoffPortfolioAnalyzer.Data;

namespace TinkPortfAnalyzer.Tests.Repositories
{
    public class DbRepositoryTestBase : IDisposable
    {
        protected PortfolioAnalyzerDb _context;
        private DbContextOptions<PortfolioAnalyzerDb> _options = 
            new DbContextOptionsBuilder<PortfolioAnalyzerDb>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options;

        public DbRepositoryTestBase() : base()
        {
            _context = new PortfolioAnalyzerDb(_options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
