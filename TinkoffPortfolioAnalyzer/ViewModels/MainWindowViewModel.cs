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
                if (_tinkoffTokens.Count() > 0)
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
                AccountTypes = _dataService.GetAccounts(CurrentTinkToken).GetAwaiter().GetResult();
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
                if (_currentAccountType != null)
                {
                    SecuritiesInfo = _dataService.GetSecuritiesInfo(_currentAccountType);
                }
            }
        }
        #endregion

        public CollectionViewSource SecuritiesViewSource { get; } = new CollectionViewSource();

        public MainWindowViewModel(IDataService dataService)
        {
            _dataService = dataService;
            OpenTokensFileCommand = new RelayCommand(OnOpenTokensFileCommandExecuted, CanOpenTokensFileCommandExecute);
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
        }
    }
}
