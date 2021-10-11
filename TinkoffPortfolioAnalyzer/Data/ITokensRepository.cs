using System.Collections.Generic;
using System.Threading.Tasks;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Data
{
    internal interface ITokensRepository
    {
        IEnumerable<TinkoffToken> GetAll();

        Task RemoveAsync(TinkoffToken tokenToDelete);

        Task AddAsync(TinkoffToken tokenToAdd);
    }
}
