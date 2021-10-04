using Library.ViewModels;
using System.Collections.Generic;
using TinkoffPortfolioAnalyzer.Models;
using TinkoffPortfolioAnalyzer.Services;

namespace TinkoffPortfolioAnalyzer.ViewModels
{
    internal class TokensManagementViewModel : BaseViewModel
    {
        private readonly ITokensService _tokensService;

        public IEnumerable<TinkoffToken> Tokens => _tokensService.GetTokens();

        public TokensManagementViewModel(ITokensService tokensService)
        {
            _tokensService = tokensService;
        }
    }
}
