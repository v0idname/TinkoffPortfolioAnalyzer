using Library.Commands;
using Library.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TinkoffPortfolioAnalyzer.Data;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.ViewModels
{
    internal class TokensManagementViewModel : BaseViewModel
    {
        private readonly ITokensRepository _tokensService;

        ObservableCollection<TinkoffToken> _tokens;
        public ObservableCollection<TinkoffToken> Tokens
        {
            get => _tokens;
            set => Set(ref _tokens, value);
        }

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

        private async void OnAddTokenCommandExecuted(object parameter)
        {
            var newToken = new TinkoffToken()
            {
                Type = SelectedTokenType,
                Value = EnteredTokenString
            };
            await _tokensService.AddAsync(newToken);
            Tokens.Add(newToken);
        }

        public ICommand DeleteTokenCommand { get; set; }

        private bool CanDeleteTokenCommandExecute(object parameter) => parameter is TinkoffToken;

        private async void OnDeleteTokenCommandExecuted(object parameter)
        {
            var delToken = (TinkoffToken)parameter;
            await _tokensService.RemoveAsync(delToken);
            Tokens.Remove(delToken);
        }

        public ICommand LoadedCommand { get; set; }

        private bool CanLoadedCommandExecute(object parameter) => true;

        private async void OnLoadedCommandExecuted(object parameter)
        {
            Tokens = new ObservableCollection<TinkoffToken>(await _tokensService.GetAllAsync().ConfigureAwait(false));
        }

        public TokensManagementViewModel(ITokensRepository tokensService)
        {
            _tokensService = tokensService;
            AddTokenCommand = new RelayCommand(OnAddTokenCommandExecuted, CanAddTokenCommandExecute);
            DeleteTokenCommand = new RelayCommand(OnDeleteTokenCommandExecuted, CanDeleteTokenCommandExecute);
            LoadedCommand = new RelayCommand(OnLoadedCommandExecuted, CanLoadedCommandExecute);
        }
    }
}
