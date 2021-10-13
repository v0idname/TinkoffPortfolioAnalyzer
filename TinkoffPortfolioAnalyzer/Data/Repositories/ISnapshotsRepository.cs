using System.Collections.Generic;
using System.Threading.Tasks;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Data.Repositories
{
    internal interface ISnapshotsRepository
    {
        public Task CreateAsync(IEnumerable<SecurityInfo> securitiesInfo);
        public Task RemoveAsync(AvailSecSnapshot snapshotToDelete);
        public Task<IEnumerable<AvailSecSnapshot>> GetAllAsync();
    }
}
