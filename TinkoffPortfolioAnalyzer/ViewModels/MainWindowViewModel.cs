using System.Collections.Generic;
using System.Linq;
using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.Trading.OpenApi.Network;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private IEnumerable<SecurityInfo> _securitiesInfo;

        public IEnumerable<SecurityInfo> SecuritiesInfo
        {
            get => _securitiesInfo;
            set => Set(ref _securitiesInfo, value);
        }

        public MainWindowViewModel()
        {
            var tradingToken = "t.zbKUmxP9SRmY14raZfJewpclgi0It5QO53eSoAalFM8uuTN82d--xg8H-swG61jd0z0lx2F9-0B8kGYLKhWzxw";

            // для работы в песочнице используйте GetSandboxConnection
            var connection = ConnectionFactory.GetConnection(tradingToken);
            var context = connection.Context;
            //var sandboxAccount = await context.RegisterAsync(BrokerAccountType.Tinkoff);
            var accounts = context.AccountsAsync().GetAwaiter().GetResult();
            var iisAccountId = accounts.First(x => x.BrokerAccountType == BrokerAccountType.TinkoffIis).BrokerAccountId;

            // вся работа происходит асинхронно через объект контекста
            var portfolio = context.PortfolioAsync(iisAccountId).GetAwaiter().GetResult();
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

            SecuritiesInfo = itemsList;
        }
    }
}
