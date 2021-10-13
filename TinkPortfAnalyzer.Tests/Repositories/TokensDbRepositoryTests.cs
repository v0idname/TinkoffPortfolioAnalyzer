using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TinkoffPortfolioAnalyzer.Data;
using TinkoffPortfolioAnalyzer.Data.Repositories;
using TinkoffPortfolioAnalyzer.Models;
using Xunit;

namespace TinkPortfAnalyzer.Tests.Repositories
{
    public class TokensDbRepositoryTests : DbRepositoryTestBase
    {
        private TinkoffToken token1 = new()
        {
            Id = 1,
            Type = TokenType.Sandbox,
            Value = "TestTokenValue1"
        };
        private TinkoffToken token2 = new()
        {
            Id = 2,
            Type = TokenType.Trading,
            Value = "TestTokenValue2"
        };

        [Fact]
        public async Task GetAllAsync_ReturnsOneToken()
        {
            _context.Tokens.Add(token1);
            await _context.SaveChangesAsync();
            var tokensDbRepo = new TokensDbRepository(_context);

            var tokens = await tokensDbRepo.GetAllAsync();

            Assert.True(tokens.Count() == 1 && tokens.Contains(token1));
        }

        [Fact]
        public async Task AddAsync_WithCorrectToken()
        {
            var tokensDbRepo = new TokensDbRepository(_context);

            await tokensDbRepo.AddAsync(token1);

            var tokens = await tokensDbRepo.GetAllAsync();
            Assert.True(tokens.Count() == 1 && tokens.Contains(token1));
        }

        [Fact]
        public async Task RemoveAsync_WithCorrectToken()
        {
            _context.Tokens.Add(token1);
            var tokensDbRepo = new TokensDbRepository(_context);

            await tokensDbRepo.RemoveAsync(token1);

            var tokens = await tokensDbRepo.GetAllAsync();
            Assert.True(tokens.Count() == 0 && !tokens.Contains(token1));
        }
    }
}
