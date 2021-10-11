using System.Collections.Generic;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Data
{
    internal interface ITokensRepository
    {
        IEnumerable<TinkoffToken> GetAll();

        void Remove(TinkoffToken tokenToDelete);

        void Add(TinkoffToken tokenToAdd);
    }
}
