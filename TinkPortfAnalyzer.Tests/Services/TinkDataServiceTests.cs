using Moq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.Trading.OpenApi.Network;
using TinkoffPortfolioAnalyzer.Models;
using TinkoffPortfolioAnalyzer.Services;
using Xunit;

namespace TinkPortfAnalyzer.Tests.Services
{
    public class TinkDataServiceTests
    {
        private SecurityInfo GetSecurityInfo(MarketInstrument instrument)
        {
            return new SecurityInfo()
            {
                Name = instrument.Name,
                Ticker = instrument.Ticker,
                InstrumentType = instrument.Type,
                Currency = instrument.Currency
            };
        }

        [Fact]
        public async Task GetAccountsAsync_Returns2CorrectAccs()
        {
            var testAccs = new ReadOnlyCollection<TinkoffAccount>(new List<TinkoffAccount>()
            {
                new TinkoffAccount(BrokerAccountType.Tinkoff, "token1"),
                new TinkoffAccount(BrokerAccountType.TinkoffIis, "token2")
            });
            var connContext = new Mock<IContext>();
            connContext.Setup(c => c.AccountsAsync()).ReturnsAsync(testAccs);
            var connService = new Mock<IConnectionService>();
            connService.Setup(c => c.GetConnectionContextAsync()).ReturnsAsync(connContext.Object);
            var dataService = new TinkDataService(connService.Object);

            var realAccs = await dataService.GetAccountsAsync();

            Assert.Equal(testAccs, realAccs);
        }

        [Fact]
        public async Task GetSecuritiesInfoAsync_WithCorrectAcc_Returns2CorrectSecurities()
        {
            // Arrange
            var positions = new List<Portfolio.Position>(2);
            var testSecurities = new List<PortfolioSecurityInfo>(2);
            for (int i = 0; i < positions.Capacity; i++)
            {
                positions.Add(new Portfolio.Position(
                    $"Name {i}", $"Figi {i}", $"Ticker {i}", $"Isin {i}",
                    InstrumentType.Bond, balance: i+1, blocked: i, expectedYield: new MoneyAmount(Currency.Chf, 0),
                    lots: i+1, averagePositionPrice: new MoneyAmount(Currency.Chf, (i+1)*100),
                    averagePositionPriceNoNkd: new MoneyAmount(Currency.Chf, i)));
                testSecurities.Add(new PortfolioSecurityInfo()
                {
                    Name = positions[i].Name,
                    Ticker = positions[i].Ticker,
                    InstrumentType = positions[i].InstrumentType,
                    Amount = (int)positions[i].Balance,
                    AveragePrice = positions[i].AveragePositionPrice.Value,
                    Currency = positions[i].AveragePositionPrice.Currency
                });
            }

            var testAcc = new TinkoffAccount(BrokerAccountType.Tinkoff, "token1");
            var connContext = new Mock<IContext>();
            connContext.Setup(c => c.PortfolioAsync(testAcc.BrokerAccountId)).ReturnsAsync(new Portfolio(positions));
            var connService = new Mock<IConnectionService>();
            connService.Setup(c => c.GetConnectionContextAsync()).ReturnsAsync(connContext.Object);
            var dataService = new TinkDataService(connService.Object);

            // Act
            var realSecurities = await dataService.GetSecuritiesInfoAsync(testAcc);

            // Assert
            Assert.Equal(testSecurities, realSecurities);
        }

        [Fact]
        public async Task GetMarketSecuritiesAsync_Returns3CorrectSecurities()
        {
            // Arrange
            var marketBonds = new List<MarketInstrument>()
            {
                new MarketInstrument(
                    figi: $"Figi bond", ticker: $"Ticker bond", isin: $"Isin bond",
                    minPriceIncrement: 1, lot: 1, currency: Currency.Chf, name: $"Name bond", type: InstrumentType.Bond)
            };
            var marketEtfs = new List<MarketInstrument>()
            {
                new MarketInstrument(
                    figi: $"Figi etf", ticker: $"Ticker etf", isin: $"Isin etf",
                    minPriceIncrement: 1, lot: 1, currency: Currency.Chf, name: $"Name etf", type: InstrumentType.Bond)
            };
            var marketStocks = new List<MarketInstrument>()
            {
                new MarketInstrument(
                    figi: $"Figi stock", ticker: $"Ticker stock", isin: $"Isin stock",
                    minPriceIncrement: 1, lot: 1, currency: Currency.Chf, name: $"Name stock", type: InstrumentType.Stock)
            };

            var testSecurities = new List<SecurityInfo>();
            testSecurities.Add(GetSecurityInfo(marketBonds[0]));
            testSecurities.Add(GetSecurityInfo(marketEtfs[0]));
            testSecurities.Add(GetSecurityInfo(marketStocks[0]));

            var connContext = new Mock<IContext>();
            connContext.Setup(c => c.MarketBondsAsync()).ReturnsAsync(
                new MarketInstrumentList(marketBonds.Count, marketBonds));
            connContext.Setup(c => c.MarketEtfsAsync()).ReturnsAsync(
                new MarketInstrumentList(marketEtfs.Count, marketEtfs));
            connContext.Setup(c => c.MarketStocksAsync()).ReturnsAsync(
                new MarketInstrumentList(marketStocks.Count, marketStocks));
            var connService = new Mock<IConnectionService>();
            connService.Setup(c => c.GetConnectionContextAsync()).ReturnsAsync(connContext.Object);
            var dataService = new TinkDataService(connService.Object);

            // Act
            var realSecurities = await dataService.GetMarketSecuritiesAsync();

            // Assert
            Assert.Equal(testSecurities, realSecurities);
        }
    }
}
