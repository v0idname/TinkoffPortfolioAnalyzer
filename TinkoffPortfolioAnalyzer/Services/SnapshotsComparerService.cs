using System.Collections.Generic;
using System.Linq;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Services
{
    public class SnapshotsComparerService : ISnapshotsComparerService
    {
        public IEnumerable<SecSnapshotDiff> Compare2Snapshots(IEnumerable<AvailSecSnapshot> snaps)
        {
            if (snaps.Count() != 2)
                return Enumerable.Empty<SecSnapshotDiff>();

            var exclusiveSecurities = new List<List<SecurityInfo>>(2);
            exclusiveSecurities.Add(snaps.First().Securities.Except(snaps.Last().Securities).ToList());
            exclusiveSecurities.Add(snaps.Last().Securities.Except(snaps.First().Securities).ToList());

            var secSnapshotDiff = new List<SecSnapshotDiff>(exclusiveSecurities.Sum(l => l.Count()));
            for (int i = 0; i < exclusiveSecurities.Count; i++)
            {
                for (int j = 0; j < exclusiveSecurities[i].Count; j++)
                {
                    secSnapshotDiff.Add(new SecSnapshotDiff()
                    {
                        Ticker = exclusiveSecurities[i][j].Ticker,
                        Name = exclusiveSecurities[i][j].Name,
                        IsSnap0Contains = i == 0,
                        IsSnap1Contains = i == 1
                    });
                }
            }

            return secSnapshotDiff;
        }
    }
}
