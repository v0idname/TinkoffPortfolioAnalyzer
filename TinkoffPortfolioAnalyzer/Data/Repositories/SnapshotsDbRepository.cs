using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Data.Repositories
{
    public class SnapshotsDbRepository : ISnapshotsRepository
    {
        private readonly PortfolioAnalyzerDb _db;

        public SnapshotsDbRepository(PortfolioAnalyzerDb db)
        {
            _db = db;
        }

        public async Task CreateAsync(IEnumerable<SecurityInfo> securitiesInfo)
        {
            await _db.Snapshots.AddAsync(new AvailSecSnapshot()
            {
                CreatedDateTime = DateTime.Now,
                Securities = securitiesInfo
            });
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<AvailSecSnapshot>> GetAllAsync()
        {
            return await _db.Snapshots.Include(item => item.Securities).ToArrayAsync();
        }

        public async Task RemoveAsync(AvailSecSnapshot snapshotToDelete)
        {
            if (!await _db.Snapshots.ContainsAsync(snapshotToDelete))
                return;
            _db.Snapshots.Remove(snapshotToDelete);
            await _db.SaveChangesAsync();
        }
    }
}
