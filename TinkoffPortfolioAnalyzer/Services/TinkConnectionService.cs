using System;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.Trading.OpenApi.Network;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Services
{
    internal class TinkConnectionService : IConnectionService
    {
        private Context _context;

        public Context GetCurrentContext() => _context;

        public async Task SetCurrentTokenAsync(TinkoffToken token)
        {
            switch (token.Type)
            {
                case TokenType.Trading:
                    {
                        var connection = ConnectionFactory.GetConnection(token.Value);
                        _context = connection.Context;
                        break;
                    }
                case TokenType.Sandbox:
                    {
                        var connection = ConnectionFactory.GetSandboxConnection(token.Value);
                        await connection.Context.RegisterAsync(BrokerAccountType.Tinkoff).ConfigureAwait(false);
                        _context = connection.Context;
                        break;
                    }
                default:
                    throw new ArgumentException("Incorrect type of token", nameof(token));
            }
        }
    }
}
