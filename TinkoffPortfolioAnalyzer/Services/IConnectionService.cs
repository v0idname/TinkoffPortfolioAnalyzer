using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Network;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Services
{
    public interface IConnectionService
    {
        public Task SetCurrentTokenAsync(TinkoffToken token);
        public Task<IContext> GetConnectionContextAsync();
    }
}
