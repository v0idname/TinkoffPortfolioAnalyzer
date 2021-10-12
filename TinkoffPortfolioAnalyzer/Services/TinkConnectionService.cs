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

        public async Task<Context> GetConnectionContextAsync()
        {
            if (_currentToken == null)
                return null;

            switch (_currentToken.Type)
            {
                case TokenType.Trading:
                    {
                        return ConnectionFactory.GetConnection(_currentToken.Value).Context;
                    }
                case TokenType.Sandbox:
                    {
                        var connection = ConnectionFactory.GetSandboxConnection(_currentToken.Value);
                        await connection.Context.RegisterAsync(BrokerAccountType.Tinkoff).ConfigureAwait(false);
                        return connection.Context;
                    }
                default:
                    throw new ArgumentException("Incorrect type of token", nameof(_currentToken));
            }
        }

        public Task SetCurrentTokenAsync(TinkoffToken token)
        {
            _currentToken = token;
            return Task.CompletedTask;
        }
    }
}
