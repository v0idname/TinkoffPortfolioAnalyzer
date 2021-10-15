using System;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.Trading.OpenApi.Network;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Services
{
    internal class TinkConnectionService : IConnectionService
    {
        private TinkoffToken _currentToken;
        private Context _context;

        public Task<Context> GetConnectionContextAsync()
        {
            return Task.FromResult(_context);
        }

        public async Task SetCurrentTokenAsync(TinkoffToken token)
        {
            _currentToken = token;
            if (_currentToken == null)
                return;

            switch (_currentToken.Type)
            {
                case TokenType.Trading:
                    {
                        _context = ConnectionFactory.GetConnection(_currentToken.Value).Context;
                        break;
                    }
                case TokenType.Sandbox:
                    {
                        var connection = ConnectionFactory.GetSandboxConnection(_currentToken.Value);
                        _context = connection.Context;
                        await connection.Context.RegisterAsync(BrokerAccountType.Tinkoff).ConfigureAwait(false);
                        break;
                    }
                default:
                    throw new ArgumentException("Incorrect type of token", nameof(_currentToken));
            }
        }
    }
}
