using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinkoffPortfolioAnalyzer.Data.Repositories;
using TinkoffPortfolioAnalyzer.Models;
using Tinkoff.Trading.OpenApi.Models;
using Xunit;

namespace TinkPortfAnalyzer.Tests.Repositories
{
    public class SnapshotsDbRepositoryTests : DbRepositoryTestBase
    {
        private IEnumerable<SecurityInfo> _securitiesInfo1 = new HashSet<SecurityInfo>
            {
                new SecurityInfo()
                {
                    Id = 1,
                    Currency = Currency.Rub,
                    InstrumentType = InstrumentType.Etf,
                    Name = "Name 1",
                    Ticker = "Ticker 1"
                },
                new SecurityInfo()
                {
                    Id = 2,
                    Currency = Currency.Usd,
                    InstrumentType = InstrumentType.Stock,
                    Name = "Name 2",
                    Ticker = "Ticker 2"
                }
            };

        private readonly TimeSpan timePrecision = TimeSpan.FromMilliseconds(200);
        private AvailSecSnapshot _testSnapshot1, _testSnapshot2;

        public SnapshotsDbRepositoryTests()
        {
            _testSnapshot1 = new()
            {
                Id = 1,
                CreatedDateTime = new DateTime(2021, 12, 31, 23, 59, 59),
                Securities = _securitiesInfo1
            };
            _testSnapshot2 = new()
            {
                Id = 2,
                CreatedDateTime = new DateTime(2021, 12, 31, 23, 59, 59),
                Securities = _securitiesInfo1
            };
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOneSnapshot()
        {
            _context.Snapshots.Add(_testSnapshot1);
            await _context.SaveChangesAsync();
            var snapsDbRepo = new SnapshotsDbRepository(_context);

            var snaps = await snapsDbRepo.GetAllAsync();

            Assert.True(snaps.Count() == 1);
            Assert.Contains(_testSnapshot1, snaps);
        }

        [Fact]
        public async Task CreateAsync_WithCorrectSecList()
        {
            var snapsDbRepo = new SnapshotsDbRepository(_context);
            var dateTimeNow = DateTime.Now;

            await snapsDbRepo.CreateAsync(_securitiesInfo1);

            var snaps = await snapsDbRepo.GetAllAsync();
            Assert.True(snaps.Count() == 1);
            Assert.Equal(dateTimeNow, snaps.FirstOrDefault().CreatedDateTime, timePrecision);
            Assert.Equal(_securitiesInfo1, snaps.FirstOrDefault().Securities);
        }

        [Fact]
        public async Task CreateAsync_With2SameSecList()
        {
            var snapsDbRepo = new SnapshotsDbRepository(_context);
            var dateTimeNow = DateTime.Now;

            await snapsDbRepo.CreateAsync(_securitiesInfo1);
            await snapsDbRepo.CreateAsync(_securitiesInfo1);

            var snaps = await snapsDbRepo.GetAllAsync();
            var snap1 = snaps.ElementAt(0);
            var snap2 = snaps.ElementAt(1);
            Assert.True(snaps.Count() == 2);
            Assert.Equal(dateTimeNow, snap1.CreatedDateTime, timePrecision);
            Assert.Equal(dateTimeNow, snap2.CreatedDateTime, timePrecision);
            Assert.Equal(snap1.CreatedDateTime, snap2.CreatedDateTime, timePrecision);
            Assert.NotEqual(snap1.CreatedDateTime, snap2.CreatedDateTime);
            Assert.Equal(snap1.Securities, snap2.Securities);
            Assert.NotEqual(snap1.Id, snap2.Id);
        }

        [Fact]
        public async Task RemoveAsync_WithCorrectSnapshot()
        {
            _context.Snapshots.Add(_testSnapshot1);
            await _context.SaveChangesAsync();
            var snapsDbRepo = new SnapshotsDbRepository(_context);

            await snapsDbRepo.RemoveAsync(_testSnapshot1);

            var snaps = await snapsDbRepo.GetAllAsync();
            Assert.Empty(snaps);
        }

        [Fact]
        public async Task RemoveAsync_With1of2CorrectSnapshots()
        {
            _context.Snapshots.Add(_testSnapshot1);
            _context.Snapshots.Add(_testSnapshot2);
            await _context.SaveChangesAsync();
            var snapsDbRepo = new SnapshotsDbRepository(_context);

            await snapsDbRepo.RemoveAsync(_testSnapshot1);

            var snaps = await snapsDbRepo.GetAllAsync();
            Assert.Contains(_testSnapshot2, snaps);
            Assert.DoesNotContain(_testSnapshot1, snaps);
        }

        [Fact]
        public async Task RemoveAsync_With2CorrectSnapshots()
        {
            _context.Snapshots.Add(_testSnapshot1);
            _context.Snapshots.Add(_testSnapshot2);
            await _context.SaveChangesAsync();
            var snapsDbRepo = new SnapshotsDbRepository(_context);

            await snapsDbRepo.RemoveAsync(_testSnapshot1);
            await snapsDbRepo.RemoveAsync(_testSnapshot2);

            var snaps = await snapsDbRepo.GetAllAsync();
            Assert.Empty(snaps);
        }

        [Fact]
        public async Task RemoveAsync_WithNotExistedToken()
        {
            var snapsDbRepo = new SnapshotsDbRepository(_context);

            await snapsDbRepo.RemoveAsync(_testSnapshot1);

            Assert.True(true);
        }
    }
}
