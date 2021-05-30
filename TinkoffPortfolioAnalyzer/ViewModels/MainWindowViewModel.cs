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
using Library.Commands;
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

        #region AvailSecSnapshots

        //TODO: ObservableCollection ??
        public List<AvailSecSnapshot> AvailSecSnapshots { get; }

        public ICommand CreateSnapshotCommand { get; }

        private bool CanCreateSnapshotCommandExecute(object p) => true;

        private void OnCreateSnapshotCommandExecuted(object p)
        {
            //var newSnapshot = new AvailSecSnapshot
            //{
            //    CreatedDateTime = DateTime.Now,
            //    Securities = new List<SecurityInfo>()
            //};

            //var bonds = _curConnectContext.MarketBondsAsync().GetAwaiter().GetResult();
            //foreach (var bond in bonds.Instruments)
            //{
            //    newSnapshot.Securities.Add(new SecurityInfo
            //    {
            //        Name = bond.Name,
            //        Ticker = bond.Ticker,
            //        InstrumentType = bond.Type,
            //        Currency = bond.Currency
            //    });
            //}
            //AvailSecSnapshots.Add(newSnapshot);
        }
        #endregion

        public CollectionViewSource SecuritiesViewSource { get; } = new CollectionViewSource();

        public MainWindowViewModel()
        {
            var tinkTokens = new List<TinkoffToken>();
            using (var sr = new StreamReader("tokens.txt"))
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

            AvailSecSnapshots = new List<AvailSecSnapshot>();
            for (int snapIndex = 0; snapIndex < 3; snapIndex++)
            {
                var newSnapshot = new AvailSecSnapshot
                {
                    CreatedDateTime = DateTime.Now,
                    Securities = new List<SecurityInfo>()
                };
                for (int i = 0; i < 10; i++)
                {
                    newSnapshot.Securities.Add(new SecurityInfo
                    {
                        Name = $"Security name {i}",
                        Ticker = $"Security ticker {i}",
                        InstrumentType = InstrumentType.Bond,
                        Currency = Currency.Rub
                    });
                }
                AvailSecSnapshots.Add(newSnapshot);
            }
            
            CreateSnapshotCommand = new RelayCommand(OnCreateSnapshotCommandExecuted, CanCreateSnapshotCommandExecute);
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
    }
}
