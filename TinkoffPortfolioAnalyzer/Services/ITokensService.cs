using System.Collections.Generic;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Services
{
    internal interface ITokensService
    {
        IEnumerable<TinkoffToken> GetTokens();

        void DeleteToken(TinkoffToken tokenToDelete);

        void AddToken(TinkoffToken tokenToAdd);
    }
}
