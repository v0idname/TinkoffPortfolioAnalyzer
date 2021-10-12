using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Data.Repositories
{
    internal interface ITokensRepository
    {
        Task<IEnumerable<TinkoffToken>> GetAllAsync();

        Task RemoveAsync(TinkoffToken tokenToDelete);

        Task AddAsync(TinkoffToken tokenToAdd);

        event EventHandler RepositoryChanged;
    }
}
