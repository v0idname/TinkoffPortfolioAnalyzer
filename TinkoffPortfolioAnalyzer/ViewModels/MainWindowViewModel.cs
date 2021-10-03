using Library.Commands;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using TinkoffPortfolioAnalyzer.Models;
using TinkoffPortfolioAnalyzer.Properties;
using TinkoffPortfolioAnalyzer.Services;

namespace TinkoffPortfolioAnalyzer.ViewModels
{
    class MainWindowViewModel : Library.ViewModels.BaseViewModel
    {
        private IDataService _dataService;

        #region SecuritiesInfo
        private IEnumerable<PortfolioSecurityInfo> _securitiesInfo;

        /// <summary>
        /// Информация о ценных бумагах из портфеля.
        /// </summary>
        public IEnumerable<PortfolioSecurityInfo> SecuritiesInfo
        {
            get => _securitiesInfo;
            set
            {
                Set(ref _securitiesInfo, value);
                SecuritiesViewSource.Source = SecuritiesInfo;
                SecuritiesViewSource.SortDescriptions.Clear();
                SecuritiesViewSource.SortDescriptions.Add(new SortDescription("TotalPrice", ListSortDirection.Descending));
            }
        }
        #endregion

        #region TinkoffTokens
        private IEnumerable<TinkoffToken> _tinkoffTokens;

        /// <summary>
        /// Токены для доступа к брокерской информации.
        /// </summary>
        public IEnumerable<TinkoffToken> TinkoffTokens
        {
            get => _tinkoffTokens;
            set
            {
                Set(ref _tinkoffTokens, value);
                if (TinkoffTokens.Count() > 0)
                    CurrentTinkToken = TinkoffTokens.First();
            }
        }

        private TinkoffToken _currentTinkoffToken;
        public TinkoffToken CurrentTinkToken
        {
            get => _currentTinkoffToken;
            set
            {
                Set(ref _currentTinkoffToken, value);
                AccountTypes = _dataService.GetAccountsAsync(CurrentTinkToken).GetAwaiter().GetResult();
                if (AccountTypes.Count() > 0)
                    CurrentAccountType = AccountTypes.First();
            }
        }
        #endregion

        #region AccountTypes
        private IEnumerable<TinkoffAccount> _accountTypes;

        /// <summary>
        /// Доступные аккаунты для выбранного токена.
        /// </summary>
        public IEnumerable<TinkoffAccount> AccountTypes
        {
            get => _accountTypes;
            set => Set(ref _accountTypes, value);
        }

        private TinkoffAccount _currentAccountType;
        public TinkoffAccount CurrentAccountType
        {
            get => _currentAccountType;
            set
            {
                Set(ref _currentAccountType, value);
            }
        }
        #endregion

        public CollectionViewSource SecuritiesViewSource { get; } = new CollectionViewSource();

        public MainWindowViewModel(IDataService dataService)
        {
            _dataService = dataService;
            PropertyChanged += MainWindowViewModel_PropertyChanged;
            OpenTokensFileCommand = new RelayCommand(OnOpenTokensFileCommandExecuted, CanOpenTokensFileCommandExecute);
            UpdateTokenList();
        }

        private async void MainWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentAccountType) && _currentAccountType != null)
            {
                SecuritiesInfo = await _dataService.GetSecuritiesInfoAsync(_currentAccountType);
            }
        }

        private void UpdateTokenList()
        {
            TinkoffTokens = _dataService.GetTokens(Settings.Default.TokenFileName);
        }

        public ICommand OpenTokensFileCommand { get; }

        private bool CanOpenTokensFileCommandExecute(object o) => true;

        private void OnOpenTokensFileCommandExecuted(object o)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            if (openFileDialog.ShowDialog() == true)
            {
                Settings.Default.TokenFileName = openFileDialog.FileName;
                Settings.Default.Save();
            }
            UpdateTokenList();
        }
    }
}
