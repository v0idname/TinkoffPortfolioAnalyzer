﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using TinkoffPortfolioAnalyzer.Models;
using TinkoffPortfolioAnalyzer.Services;

namespace TinkoffPortfolioAnalyzer.ViewModels
{
    class MainWindowViewModel : Library.ViewModels.BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly ITokensService _tokensService;

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
                if (TinkoffTokens.Any())
                    CurrentTinkToken = TinkoffTokens.First();
            }
        }

        private TinkoffToken _currentTinkoffToken;
        public TinkoffToken CurrentTinkToken
        {
            get => _currentTinkoffToken;
            set => Set(ref _currentTinkoffToken, value);
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
            set => Set(ref _currentAccountType, value);
        }
        #endregion

        public CollectionViewSource SecuritiesViewSource { get; } = new CollectionViewSource();

        public MainWindowViewModel(IDataService dataService, ITokensService tokensService)
        {
            _dataService = dataService;
            _tokensService = tokensService;
            PropertyChanged += MainWindowViewModel_PropertyChanged;
            UpdateTokenList();
        }

        private async void MainWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentAccountType))
            {
                SecuritiesInfo = await _dataService.GetSecuritiesInfoAsync(CurrentAccountType);
            }
            else if (e.PropertyName == nameof(CurrentTinkToken))
            {
                AccountTypes = await _dataService.GetAccountsAsync(CurrentTinkToken);
                if (AccountTypes.Count() > 0)
                    CurrentAccountType = AccountTypes.First();
            }
        }

        private void UpdateTokenList()
        {
            TinkoffTokens = _tokensService.GetTokens();
        }
    }
}
