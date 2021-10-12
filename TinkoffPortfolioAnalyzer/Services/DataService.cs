using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Services
{
    internal class DataService : IDataService
    {
        private readonly IConnectionService _conService;

        public DataService(IConnectionService conService)
        {
            _conService = conService;
        }
        
        public async Task SetCurrentToken(TinkoffToken token)
        {
            await _conService.SetCurrentTokenAsync(token);
        }

        public async Task<IEnumerable<TinkoffAccount>> GetAccountsAsync()
        {
            var accs = await _conService.GetCurrentContext().AccountsAsync().ConfigureAwait(false);
            var accList = new List<TinkoffAccount>(accs.Count);
            foreach (var acc in accs)
                accList.Add(new TinkoffAccount(acc));
            return accList;
        }

        public async Task<IEnumerable<PortfolioSecurityInfo>> GetSecuritiesInfoAsync(Account acc)
        {
            if (acc == null)
                return new List<PortfolioSecurityInfo>();

            var portfolio = await _conService.GetCurrentContext().PortfolioAsync(acc.BrokerAccountId).ConfigureAwait(false);
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
            secList.AddMarketInstList(await _conService.GetCurrentContext().MarketBondsAsync().ConfigureAwait(false));
            secList.AddMarketInstList(await _conService.GetCurrentContext().MarketEtfsAsync().ConfigureAwait(false));
            secList.AddMarketInstList(await _conService.GetCurrentContext().MarketStocksAsync().ConfigureAwait(false));
            return secList;
        }
    }
}
