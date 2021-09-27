using System.Collections.Generic;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Services
{
    internal interface IDataService
    {
        Task<IEnumerable<TinkoffAccount>> GetAccounts(TinkoffToken token);
        IEnumerable<PortfolioSecurityInfo> GetSecuritiesInfo(Account acc);
        IEnumerable<TinkoffToken> GetTokens(string fileName);
        Task<SecurityInfoList> GetMarketSecuritiesAsync();
    }
}