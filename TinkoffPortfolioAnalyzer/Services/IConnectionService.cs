using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Network;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Services
{
    internal interface IConnectionService
    {
        public Task SetCurrentTokenAsync(TinkoffToken token);
        public Task<Context> GetConnectionContextAsync();
    }
}
