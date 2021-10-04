using System.Collections.Generic;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Services
{
    internal interface IDataService
    {
        Task<IEnumerable<TinkoffAccount>> GetAccountsAsync(TinkoffToken token);
        Task<IEnumerable<PortfolioSecurityInfo>> GetSecuritiesInfoAsync(Account acc);
        Task<SecurityInfoList> GetMarketSecuritiesAsync();
    }
}