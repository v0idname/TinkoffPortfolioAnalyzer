using System;
using System.Collections.Generic;
using System.Linq;
using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.Trading.OpenApi.Network;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private Context _curConnectContext;

        #region SecuritiesInfo
        private IEnumerable<SecurityInfo> _securitiesInfo;

        /// <summary>
        /// Информация о ценных бумагах из портфеля.
        /// </summary>
        public IEnumerable<SecurityInfo> SecuritiesInfo
        {
            get => _securitiesInfo;
            set => Set(ref _securitiesInfo, value);
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
                SecuritiesInfo = GetSecuritiesInfo(_currentTinkoffToken, _currentAccountType);
            }
        }
        #endregion

        public MainWindowViewModel()
        {
            var tinkTokens = new List<TinkoffToken>(2);
            tinkTokens.Add(new TinkoffToken()
            {
                Type = TokenType.Trading,
                Value = "t.zbKUmxP9SRmY14raZfJewpclgi0It5QO53eSoAalFM8uuTN82d--xg8H-swG61jd0z0lx2F9-0B8kGYLKhWzxw"
            });
            tinkTokens.Add(new TinkoffToken()
            {
                Type = TokenType.Sandbox,
                Value = "t.qnjT83GCBu8TWdiXRcuqCnUXoh_Z-sstY0eyhq9duzZ8uKqZfOzLeculB8A3lou_Rl7pfMF6k8wvIp-6kNVBxQ"
            });
            TinkoffTokens = tinkTokens;
        }

        private IEnumerable<TinkoffAccount> GetAccounts(TinkoffToken token)
        {
            if (token.Type == TokenType.Sandbox)
                throw new NotImplementedException();

            var connection = ConnectionFactory.GetConnection(token.Value);
            _curConnectContext = connection.Context;

            foreach(var acc in _curConnectContext.AccountsAsync().GetAwaiter().GetResult())
            {
                yield return new TinkoffAccount(acc);
            }
        }

        private IEnumerable<SecurityInfo> GetSecuritiesInfo(TinkoffToken token, Account acc)
        {
            if (token.Type == TokenType.Sandbox)
                throw new NotImplementedException();

            var portfolio = _curConnectContext.PortfolioAsync(acc.BrokerAccountId).GetAwaiter().GetResult();
            var itemsList = new List<SecurityInfo>(portfolio.Positions.Count);
            foreach (var item in portfolio.Positions)
            {
                itemsList.Add(new SecurityInfo()
                {
                    Ticker = item.Ticker,
                    InstrumentType = item.InstrumentType,
                    Price = item.AveragePositionPrice.Value,
                    Currency = item.AveragePositionPrice.Currency
                });
            }

            return itemsList;
        }
    }
}
