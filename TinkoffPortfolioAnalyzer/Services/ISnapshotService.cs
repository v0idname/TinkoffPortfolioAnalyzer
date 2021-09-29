using System.Collections.Generic;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Services
{
    internal interface ISnapshotService
    {
        public void CreateSnapshot(SecurityInfoList securityInfoList);
        public IEnumerable<AvailSecSnapshot> GetSnapshots();
    }
}
