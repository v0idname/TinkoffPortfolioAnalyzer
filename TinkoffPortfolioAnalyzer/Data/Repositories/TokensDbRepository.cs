using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Data.Repositories
{
    public class TokensDbRepository : ITokensRepository
    {
        private readonly PortfolioAnalyzerDb _db;

        public TokensDbRepository(PortfolioAnalyzerDb db)
        {
            _db = db;
        }

        public event EventHandler RepositoryChanged;

        public async Task AddAsync(TinkoffToken tokenToAdd)
        {
            if (await _db.Tokens.ContainsAsync(tokenToAdd))
                return;
            var res = await _db.Tokens.AddAsync(tokenToAdd);
            await _db.SaveChangesAsync();
            RepositoryChanged?.Invoke(this, new EventArgs());
        }

        public async Task<IEnumerable<TinkoffToken>> GetAllAsync()
        {
            return await _db.Tokens.ToListAsync();
        }

        public async Task RemoveAsync(TinkoffToken tokenToDelete)
        {
            if (!await _db.Tokens.ContainsAsync(tokenToDelete))
                return;
            _db.Tokens.Remove(tokenToDelete);
            await _db.SaveChangesAsync();
            RepositoryChanged?.Invoke(this, new EventArgs());
        }
    }
}
