using Library.Commands;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.Trading.OpenApi.Network;
using TinkoffPortfolioAnalyzer.Models;
using TinkoffPortfolioAnalyzer.Properties;

namespace TinkoffPortfolioAnalyzer.ViewModels
{
    class MainWindowViewModel : Library.ViewModels.BaseViewModel
    {
        private Context _curConnectContext;

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
                AccountTypes = GetAccounts(CurrentTinkToken).GetAwaiter().GetResult();
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
                    SecuritiesInfo = GetSecuritiesInfo(_currentAccountType);
                    SecuritiesPlotModel = GetSecuritiesPlotModel();
                }
            }
        }
        #endregion

        #region SecuritiesPlotModel
        private PlotModel _secStats;

        /// <summary>
        /// Доступные аккаунты для выбранного токена.
        /// </summary>
        public PlotModel SecuritiesPlotModel
        {
            get => _secStats;
            set => Set(ref _secStats, value);
        }
        #endregion

        public CollectionViewSource SecuritiesViewSource { get; } = new CollectionViewSource();

        public MainWindowViewModel()
        {
            OpenTokensFileCommand = new RelayCommand(OnOpenTokensFileCommandExecuted, CanOpenTokensFileCommandExecute);

            var tinkTokens = new List<TinkoffToken>();
            try
            {
                using (var sr = new StreamReader(Settings.Default.TokenFileName))
                {
                    var fileLine = sr.ReadLine();
                    var fileLineArr = fileLine.Split(" ");
                    if (fileLineArr[0] == TokenType.Trading.ToString())
                        tinkTokens.Add(new TinkoffToken()
                        {
                            Type = TokenType.Trading,
                            Value = fileLineArr[1]
                        });
                }
                TinkoffTokens = tinkTokens;
            }
            catch (ArgumentException)
            {

            }
        }

        private async Task<IEnumerable<TinkoffAccount>> GetAccounts(TinkoffToken token)
        {
            switch (token.Type)
            {
                case TokenType.Trading:
                    {
                        var connection = ConnectionFactory.GetConnection(token.Value);
                        _curConnectContext = connection.Context;
                        break;
                    }

                case TokenType.Sandbox:
                    {
                        var connection = ConnectionFactory.GetSandboxConnection(token.Value);
                        await connection.Context.RegisterAsync(BrokerAccountType.Tinkoff);
                        _curConnectContext = connection.Context;
                        break;
                    }

                default:
                    throw new ArgumentException("Incorrect type of token", nameof(token));
            }

            var accList = new List<TinkoffAccount>();
            var accs = _curConnectContext.AccountsAsync().GetAwaiter().GetResult();
            foreach (var acc in accs)
                accList.Add(new TinkoffAccount(acc));
            return accList;
        }

        private IEnumerable<PortfolioSecurityInfo> GetSecuritiesInfo(Account acc)
        {
            var portfolio = _curConnectContext.PortfolioAsync(acc.BrokerAccountId).GetAwaiter().GetResult();
            var itemsList = new List<PortfolioSecurityInfo>(portfolio.Positions.Count);
            foreach (var item in portfolio.Positions)
            {
                itemsList.Add(new PortfolioSecurityInfo()
                {
                    Name = item.Name,
                    Ticker = item.Ticker,
                    InstrumentType = item.InstrumentType,
                    Price = item.AveragePositionPrice.Value,
                    Currency = item.AveragePositionPrice.Currency,
                    Amount = (int)Math.Round(item.Balance)
                });
            }

            return itemsList;
        }

        private PlotModel GetSecuritiesPlotModel()
        {
            var plotModel = new PlotModel();
            var pieSeries = new PieSeries()
            {
                InsideLabelFormat = "",
                OutsideLabelFormat = "{1}: {2:0.0}%",
            };
            
            foreach (var security in SecuritiesInfo.OrderBy(x => x.TotalPrice))
            {
                pieSeries.Slices.Add(new PieSlice(security.Name, decimal.ToDouble(security.TotalPrice)));
            }
            plotModel.Series.Add(pieSeries);
            return plotModel;
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
