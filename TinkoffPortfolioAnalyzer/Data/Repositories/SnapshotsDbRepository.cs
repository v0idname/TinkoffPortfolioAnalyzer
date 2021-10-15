using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var secsToAdd = securitiesInfo.Except(_db.AvailSecurities).ToList();
            await _db.AvailSecurities.AddRangeAsync(secsToAdd);
            await _db.SaveChangesAsync();

            var intersect = _db.AvailSecurities.AsEnumerable().Intersect(securitiesInfo).ToList();

            //var securitiesInfoWithId = new List<SecurityInfo>(securitiesInfo.Count());
            //foreach (var sec in securitiesInfo)
            //{
            //    securitiesInfoWithId.AddRange(_db.AvailSecurities.Where(s => s.Equals(sec)).ToList());
            //}

            await _db.Snapshots.AddAsync(new AvailSecSnapshot()
            {
                CreatedDateTime = DateTime.Now,
                Securities = intersect
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
