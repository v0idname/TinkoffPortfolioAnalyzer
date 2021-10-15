using System;
using System.Linq;
using System.Threading.Tasks;
using TinkoffPortfolioAnalyzer.Data.Repositories;
using TinkoffPortfolioAnalyzer.Models;
using Xunit;

namespace TinkPortfAnalyzer.Tests.Repositories
{
    public class TokensDbRepositoryTests : DbRepositoryTestBase
    {
        private TinkoffToken testToken1 = new()
        {
            Id = 1,
            Type = TokenType.Sandbox,
            Value = "TestTokenValue1"
        };
        private TinkoffToken testToken2 = new()
        {
            Id = 2,
            Type = TokenType.Trading,
            Value = "TestTokenValue2"
        };

        [Fact]
        public async Task GetAllAsync_ReturnsOneToken()
        {
            _context.Tokens.Add(testToken1);
            await _context.SaveChangesAsync();
            var tokensDbRepo = new TokensDbRepository(_context);

            var tokens = await tokensDbRepo.GetAllAsync();

            Assert.Single(tokens);
            Assert.Contains(testToken1, tokens);
        }

        [Fact]
        public async Task AddAsync_WithCorrectToken()
        {
            var tokensDbRepo = new TokensDbRepository(_context);

            await Assert.RaisesAsync<EventArgs>(
                h => tokensDbRepo.RepositoryChanged += h,
                h => tokensDbRepo.RepositoryChanged -= h,
                async () => await tokensDbRepo.AddAsync(testToken1));

            var tokens = await tokensDbRepo.GetAllAsync();
            Assert.True(tokens.Count() == 1);
            Assert.Contains(testToken1, tokens);
        }

        [Fact]
        public async Task AddAsync_WithDublicateToken()
        {
            var tokensDbRepo = new TokensDbRepository(_context);

            await tokensDbRepo.AddAsync(testToken1);
            await tokensDbRepo.AddAsync(testToken1);

            var tokens = await tokensDbRepo.GetAllAsync();
            Assert.Single(tokens);
            Assert.Contains(testToken1, tokens);
        }

        [Fact]
        public async Task RemoveAsync_WithCorrectToken()
        {
            _context.Tokens.Add(testToken1);
            _context.Tokens.Add(testToken2);
            await _context.SaveChangesAsync();
            var tokensDbRepo = new TokensDbRepository(_context);

            await Assert.RaisesAsync<EventArgs>(
                h => tokensDbRepo.RepositoryChanged += h,
                h => tokensDbRepo.RepositoryChanged -= h,
                async () => await tokensDbRepo.RemoveAsync(testToken1));
            
            var tokens = await tokensDbRepo.GetAllAsync();
            Assert.Single(tokens);
            Assert.DoesNotContain(testToken1, tokens);
        }

        [Fact]
        public async Task RemoveAsync_WithNotExistedToken()
        {
            _context.Tokens.Add(testToken1);
            _context.Tokens.Add(testToken2);
            await _context.SaveChangesAsync();
            var tokensDbRepo = new TokensDbRepository(_context);

            await tokensDbRepo.RemoveAsync(new TinkoffToken());

            var tokens = await tokensDbRepo.GetAllAsync();
            Assert.True(tokens.Count() == 2);
        }
    }
}
