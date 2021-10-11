using Library.Commands;
using Library.ViewModels;
using System.Collections.Generic;
using System.Windows.Input;
using TinkoffPortfolioAnalyzer.Data;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.ViewModels
{
    internal class TokensManagementViewModel : BaseViewModel
    {
        private readonly ITokensRepository _tokensService;

        public IEnumerable<TinkoffToken> Tokens => _tokensService.GetAll();

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
            _tokensService.Add(new TinkoffToken()
            {
                Type = SelectedTokenType,
                Value = EnteredTokenString
            });
        }

        public ICommand DeleteTokenCommand { get; set; }

        private bool CanDeleteTokenCommandExecute(object parameter) => parameter is TinkoffToken;

        private void OnDeleteTokenCommandExecuted(object parameter)
        {
            _tokensService.Remove((TinkoffToken)parameter);
        }

        public TokensManagementViewModel(ITokensRepository tokensService)
        {
            _tokensService = tokensService;
            AddTokenCommand = new RelayCommand(OnAddTokenCommandExecuted, CanAddTokenCommandExecute);
            DeleteTokenCommand = new RelayCommand(OnDeleteTokenCommandExecuted, CanDeleteTokenCommandExecute);
        }
    }
}
