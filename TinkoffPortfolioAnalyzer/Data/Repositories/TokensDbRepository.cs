using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Data.Repositories
{
    internal class TokensDbRepository : ITokensRepository
    {
        private readonly PortfolioAnalyzerDb _db;
        private readonly DbSet<TinkoffToken> _tokens;

        public TokensDbRepository(PortfolioAnalyzerDb db)
        {
            _db = db;
            _tokens = _db.Set<TinkoffToken>();
        }

        public event EventHandler RepositoryChanged;

        public async Task AddAsync(TinkoffToken tokenToAdd)
        {
            await _tokens.AddAsync(tokenToAdd);
            await _db.SaveChangesAsync();
            RepositoryChanged?.Invoke(this, new EventArgs());
        }

        public async Task<IEnumerable<TinkoffToken>> GetAllAsync()
        {
            return await _tokens.ToListAsync();
        }

        public async Task RemoveAsync(TinkoffToken tokenToDelete)
        {
            _tokens.Remove(tokenToDelete);
            await _db.SaveChangesAsync();
            RepositoryChanged?.Invoke(this, new EventArgs());
        }
    }
}
