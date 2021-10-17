using System.Collections.Generic;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Services
{
    public interface IDataService
    {
        Task SetCurrentTokenAsync(TinkoffToken token);
        Task<IEnumerable<TinkoffAccount>> GetAccountsAsync();
        Task<IEnumerable<PortfolioSecurityInfo>> GetSecuritiesInfoAsync(Account acc);
        Task<IEnumerable<SecurityInfo>> GetMarketSecuritiesAsync();
    }
}