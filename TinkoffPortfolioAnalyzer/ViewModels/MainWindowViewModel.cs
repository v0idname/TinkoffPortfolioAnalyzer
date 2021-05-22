using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Data;
using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.Trading.OpenApi.Network;
using TinkoffPortfolioAnalyzer.Models;

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
            set => Set(ref _tinkoffTokens, value);
        }

        private TinkoffToken _currentTinkoffToken;
        public TinkoffToken CurrentTinkToken
        {
            get => _currentTinkoffToken;
            set
            {
                Set(ref _currentTinkoffToken, value);
                AccountTypes = GetAccounts(_currentTinkoffToken);
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
                    SecuritiesInfo = GetSecuritiesInfo(_currentTinkoffToken, _currentAccountType);
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
            var tinkTokens = new List<TinkoffToken>();
            using (var sr = new StreamReader("../../../tokens.txt"))
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

        private IEnumerable<TinkoffAccount> GetAccounts(TinkoffToken token)
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
                        connection.Context.RegisterAsync(BrokerAccountType.Tinkoff);
                        _curConnectContext = connection.Context;
                        break;
                    }

                default:
                    throw new ArgumentException("Incorrect type of token", nameof(token));
            }

            foreach (var acc in _curConnectContext.AccountsAsync().GetAwaiter().GetResult())
            {
                yield return new TinkoffAccount(acc);
            }
        }

        private IEnumerable<PortfolioSecurityInfo> GetSecuritiesInfo(TinkoffToken token, Account acc)
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
    }
}
