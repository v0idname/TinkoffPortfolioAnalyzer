using System.Collections.Generic;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Services
{
    public interface ISnapshotsComparerService
    {
        IEnumerable<SecSnapshotDiff> Compare2Snapshots(IEnumerable<AvailSecSnapshot> snaps);
    }
}
