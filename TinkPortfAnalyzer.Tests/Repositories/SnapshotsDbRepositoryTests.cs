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
        private readonly TimeSpan timePrecision = TimeSpan.FromMilliseconds(500);
        private AvailSecSnapshot _testSnapshot1, _testSnapshot2;

        private SecurityInfo[] CreateSameSecurities(int secCount)
        {
            return Enumerable.Range(0, secCount)
                .Select(i => new SecurityInfo()
                {
                    Id = 0,
                    Currency = Currency.Rub,
                    InstrumentType = InstrumentType.Etf,
                    Name = "Name 1",
                    Ticker = "Ticker 1"
                })
                .ToArray();
        }

        private SecurityInfo[] CreateDiffSecurities(int secCount)
        {
            return Enumerable.Range(0, secCount)
                .Select(i => new SecurityInfo()
                {
                    Id = 0,
                    Currency = Currency.Rub,
                    InstrumentType = InstrumentType.Etf,
                    Name = $"Name {i}",
                    Ticker = $"Ticker {i}"
                })
                .ToArray();
        }

        public SnapshotsDbRepositoryTests()
        {
            _testSnapshot1 = new()
            {
                Id = 1,
                CreatedDateTime = new DateTime(2021, 12, 31, 23, 59, 59),
                Securities = CreateDiffSecurities(2)
            };
            _testSnapshot2 = new()
            {
                Id = 2,
                CreatedDateTime = new DateTime(2021, 12, 31, 23, 59, 59),
                Securities = CreateDiffSecurities(2)
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
            var sec1 = CreateDiffSecurities(2);
            var snapsDbRepo = new SnapshotsDbRepository(_context);
            var dateTimeNow = DateTime.Now;

            await snapsDbRepo.CreateAsync(sec1);

            var snaps = await snapsDbRepo.GetAllAsync();
            Assert.True(snaps.Count() == 1);
            Assert.Equal(dateTimeNow, snaps.FirstOrDefault().CreatedDateTime, timePrecision);
            Assert.Equal(sec1[0], snaps.FirstOrDefault().Securities.ElementAt(0));
            Assert.Equal(sec1[1], snaps.FirstOrDefault().Securities.ElementAt(1));
        }

        [Fact]
        public async Task CreateAsync_With2SameSecurities()
        {
            var sec1 = CreateSameSecurities(2);
            var snapsDbRepo = new SnapshotsDbRepository(_context);
            var dateTimeNow = DateTime.Now;

            await snapsDbRepo.CreateAsync(sec1);

            var snaps = await snapsDbRepo.GetAllAsync();
            var snap1 = snaps.ElementAt(0);
            Assert.True(snaps.Count() == 1);
            Assert.Equal(dateTimeNow, snap1.CreatedDateTime, timePrecision);
            Assert.True(snap1.Securities.Count() == 1);
        }

        [Fact]
        public async Task CreateAsync_With2SameSecButDiffId()
        {
            var sec1 = CreateSameSecurities(2);
            var sec2 = CreateSameSecurities(2);
            sec2[0].Id = 3;
            sec2[1].Id = 4;
            var snapsDbRepo = new SnapshotsDbRepository(_context);
            var dateTimeNow = DateTime.Now;

            await snapsDbRepo.CreateAsync(sec1);
            await snapsDbRepo.CreateAsync(sec2);

            var snaps = await snapsDbRepo.GetAllAsync();
            var snap1 = snaps.ElementAt(0);
            var snap2 = snaps.ElementAt(1);
            Assert.True(snaps.Count() == 2);
            Assert.Equal(dateTimeNow, snap1.CreatedDateTime, timePrecision);
            //Assert.Equal(dateTimeNow, snap2.CreatedDateTime, timePrecision);
            Assert.Equal(snap1.CreatedDateTime, snap2.CreatedDateTime, timePrecision);
            Assert.NotEqual(snap1.Id, snap2.Id);
            Assert.Equal(snap1.Securities.ElementAt(0), snap2.Securities.ElementAt(0));
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
