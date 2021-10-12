using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Data.Repositories
{
    internal class SnapshotsDbRepository : ISnapshotsRepository
    {
        private readonly PortfolioAnalyzerDb _db;
        private DbSet<AvailSecSnapshot> _snapshots;

        public SnapshotsDbRepository(PortfolioAnalyzerDb db)
        {
            _db = db;
            _snapshots = _db.Set<AvailSecSnapshot>();
        }

        public async Task CreateAsync(SecurityInfoList securityInfoList)
        {
            await _snapshots.AddAsync(new AvailSecSnapshot()
            {
                CreatedDateTime = DateTime.Now,
                Securities = securityInfoList.List
            });
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<AvailSecSnapshot>> GetAllAsync()
        {
            return await _snapshots.Include(item => item.Securities).ToArrayAsync();
        }

        public async Task RemoveAsync(AvailSecSnapshot snapshotToDelete)
        {
            await Task.Run(() => _snapshots.Remove(snapshotToDelete));
            await _db.SaveChangesAsync();
        }
    }
}
