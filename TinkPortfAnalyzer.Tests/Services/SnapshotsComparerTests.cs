using System;
using System.Linq;
using Tinkoff.Trading.OpenApi.Models;
using TinkoffPortfolioAnalyzer.Models;
using TinkoffPortfolioAnalyzer.Services;
using Xunit;

namespace TinkPortfAnalyzer.Tests.Services
{
    public class SnapshotsComparerTests
    {
        public SnapshotsComparerTests()
        {
        }

        private AvailSecSnapshot[] CreateSameSnaps(int snapCount, int secCount)
        {
            return Enumerable.Range(0, snapCount)
                .Select(i => new AvailSecSnapshot()
                {
                    Id = 0,
                    CreatedDateTime = new DateTime(2021, 12, 31, 23, 59, 59),
                    Securities = Enumerable.Range(0, secCount)
                    .Select(i => new SecurityInfo()
                    {
                        Id = 0,
                        Currency = Currency.Rub,
                        InstrumentType = InstrumentType.Etf,
                        Name = $"Name",
                        Ticker = $"Ticker"
                    })
                    .ToArray()
                })
                .ToArray();
        }

        [Fact]
        public void Compare2SnapsBySecurities_WithSameSnaps_ReturnsEmptyDiff()
        {
            var snaps = CreateSameSnaps(snapCount: 2, secCount: 2);

            var diff = SnapshotsComparerService.Compare2SnapsBySecurities(snaps);

            Assert.Empty(diff);
        }

        [Fact]
        public void Compare2SnapsBySecurities_WithDiffId_ReturnsAllAsDiff()
        {
            var snaps = CreateSameSnaps(snapCount: 2, secCount: 1);
            snaps[0].Securities.ElementAt(0).Id = 0;
            snaps[1].Securities.ElementAt(0).Id = 1;

            var diff = SnapshotsComparerService.Compare2SnapsBySecurities(snaps);

            Assert.True(diff.Count() == 2);
        }

        [Fact]
        public void Compare2SnapsBySecurities_WithDiffTicker_ReturnsAllAsDiff()
        {
            const string ticker1 = "Ticker 1";
            const string ticker2 = "Ticker 2";
            var snaps = CreateSameSnaps(snapCount: 2, secCount: 1);
            snaps[0].Securities.ElementAt(0).Ticker = ticker1;
            snaps[1].Securities.ElementAt(0).Ticker = ticker2;

            var diff = SnapshotsComparerService.Compare2SnapsBySecurities(snaps);

            Assert.True(diff.Count() == 2);
            Assert.True(diff.Single(d => d.Ticker == ticker1).IsSnap0Contains);
            Assert.False(diff.Single(d => d.Ticker == ticker1).IsSnap1Contains);
            Assert.False(diff.Single(d => d.Ticker == ticker2).IsSnap0Contains);
            Assert.True(diff.Single(d => d.Ticker == ticker2).IsSnap1Contains);
        }

        [Fact]
        public void Compare2SnapsBySecurities_WithDiffTicker_Returns1Diff()
        {
            const string ticker = "New Ticker";
            var snaps = CreateSameSnaps(snapCount: 2, secCount: 2);
            snaps[1].Securities.ElementAt(1).Ticker = ticker;

            var diff = SnapshotsComparerService.Compare2SnapsBySecurities(snaps);

            Assert.True(diff.Count() == 1);
            Assert.False(diff.Single(d => d.Ticker == ticker).IsSnap0Contains);
            Assert.True(diff.Single(d => d.Ticker == ticker).IsSnap1Contains);
        }
    }
}
