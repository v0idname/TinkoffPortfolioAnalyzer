using Library.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using TinkoffPortfolioAnalyzer.Data.Entities;
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
            //await _db.Database.EnsureDeletedAsync();
            await _db.Database.MigrateAsync();
            //await InitTokensAsync();
            //await InitSnapshotsAsync();
            await _db.SaveChangesAsync();
        }

        private async Task InitTokensAsync()
        {
            Random rnd = new Random();
            var tokens = Enumerable.Range(0, 100).Select(t => new TinkoffToken()
            {
                Type = rnd.NextEnumItem<TokenType>(),
                Value = Guid.NewGuid().ToString()
            });
            await _db.Tokens.AddRangeAsync(tokens);
        }

        private async Task InitSnapshotsAsync()
        {
            Random rnd = new Random();
            var securities = Enumerable.Range(0, 10).Select(s => new SecurityInfo()
            { 
                Currency = rnd.NextEnumItem<Currency>(),
                InstrumentType = rnd.NextEnumItem<InstrumentType>(),
                Name = $"Name {s}",
                Ticker = $"{Guid.NewGuid()}"
            });

            var snapshots = Enumerable.Range(0, 100).Select(t => new AvailSecSnapshot()
            {
                CreatedDateTime = DateTime.Now,
                Securities = securities.ToArray()
            });
            await _db.Snapshots.AddRangeAsync(snapshots);
        }
    }
}
