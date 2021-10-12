using System.Collections.Generic;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Data.Repositories
{
    internal interface ISnapshotsRepository
    {
        public void Create(SecurityInfoList securityInfoList);
        public void Remove(AvailSecSnapshot snapshotToDelete);
        public IEnumerable<AvailSecSnapshot> GetAll();
    }
}
