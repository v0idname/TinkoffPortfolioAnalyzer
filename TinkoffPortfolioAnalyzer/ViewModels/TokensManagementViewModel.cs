using Library.Commands;
using Library.ViewModels;
using System.Collections.Generic;
using System.Windows.Input;
using TinkoffPortfolioAnalyzer.Models;
using TinkoffPortfolioAnalyzer.Services;

namespace TinkoffPortfolioAnalyzer.ViewModels
{
    internal class TokensManagementViewModel : BaseViewModel
    {
        private readonly ITokensService _tokensService;

        public IEnumerable<TinkoffToken> Tokens => _tokensService.GetTokens();

        private TokenType _selectedTokenType;
        public TokenType SelectedTokenType 
        {
            get => _selectedTokenType;
            set => Set(ref _selectedTokenType, value);
        }

        private string _enteredTokenString;
        public string EnteredTokenString
        {
            get => _enteredTokenString;
            set => Set(ref _enteredTokenString, value);
        }

        public ICommand AddTokenCommand { get; set; }

        private bool CanAddTokenCommandExecute(object parameter) => true;

        private void OnAddTokenCommandExecuted(object parameter)
        {
            _tokensService.AddToken(new TinkoffToken()
            {
                Type = SelectedTokenType,
                Value = EnteredTokenString
            });
        }

        public ICommand DeleteTokenCommand { get; set; }

        private bool CanDeleteTokenCommandExecute(object parameter) => parameter is TinkoffToken;

        private void OnDeleteTokenCommandExecuted(object parameter)
        {
            _tokensService.DeleteToken((TinkoffToken)parameter);
        }

        public TokensManagementViewModel(ITokensService tokensService)
        {
            _tokensService = tokensService;
            AddTokenCommand = new RelayCommand(OnAddTokenCommandExecuted, CanAddTokenCommandExecute);
            DeleteTokenCommand = new RelayCommand(OnDeleteTokenCommandExecuted, CanDeleteTokenCommandExecute);
        }
    }
}
