using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.Trading.OpenApi.Network;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Services
{
    internal class DataService : IDataService
    {
        private Context _curConnectContext;

        public async Task<IEnumerable<TinkoffAccount>> GetAccountsAsync(TinkoffToken token)
        {
            var accList = new List<TinkoffAccount>();

            if (token == null)
                return accList;

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
                        await connection.Context.RegisterAsync(BrokerAccountType.Tinkoff).ConfigureAwait(false);
                        _curConnectContext = connection.Context;
                        break;
                    }

                default:
                    throw new ArgumentException("Incorrect type of token", nameof(token));
            }

            var accs = await _curConnectContext.AccountsAsync().ConfigureAwait(false);
            foreach (var acc in accs)
                accList.Add(new TinkoffAccount(acc));
            return accList;
        }

        public async Task<IEnumerable<PortfolioSecurityInfo>> GetSecuritiesInfoAsync(Account acc)
        {
            var portfolio = await _curConnectContext.PortfolioAsync(acc.BrokerAccountId).ConfigureAwait(false);
            var itemsList = new List<PortfolioSecurityInfo>(portfolio.Positions.Count);
            foreach (var item in portfolio.Positions)
            {
                itemsList.Add(new PortfolioSecurityInfo()
                {
                    Name = item.Name,
                    Ticker = item.Ticker,
                    InstrumentType = item.InstrumentType,
                    AveragePrice = item.AveragePositionPrice.Value,
                    Currency = item.AveragePositionPrice.Currency,
                    Amount = (int)Math.Round(item.Balance)
                });
            }

            return itemsList;
        }

        public async Task<SecurityInfoList> GetMarketSecuritiesAsync()
        {
            var secList = new SecurityInfoList();
            secList.AddMarketInstList(await _curConnectContext.MarketBondsAsync().ConfigureAwait(false));
            secList.AddMarketInstList(await _curConnectContext.MarketEtfsAsync().ConfigureAwait(false));
            secList.AddMarketInstList(await _curConnectContext.MarketStocksAsync().ConfigureAwait(false));
            return secList;
        }
    }
}
