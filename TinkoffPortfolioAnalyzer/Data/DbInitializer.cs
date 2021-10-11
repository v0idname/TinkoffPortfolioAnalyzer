using Library.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Data
{
    class DbInitializer
    {
        private readonly PortfolioAnalyzerDb _db;

        public DbInitializer(PortfolioAnalyzerDb db)
        {
            _db = db;
        }

        public async Task InitAsync()
        {
            await _db.Database.EnsureDeletedAsync();
            await _db.Database.MigrateAsync();
            Random rnd = new Random();
            var tokens = Enumerable.Range(0, 100).Select(t => new TokenEntity()
            {
                Type = rnd.NextEnumItem<TokenType>(),
                Value = Guid.NewGuid().ToString()
            });
            await _db.Tokens.AddRangeAsync(tokens);
            await _db.SaveChangesAsync();
        }
    }
}
