using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using TinkoffPortfolioAnalyzer.Data;
using Microsoft.EntityFrameworkCore;
using TinkoffPortfolioAnalyzer.Models;
using TinkoffPortfolioAnalyzer.Data.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace TinkPortfAnalyzer.Tests
{
    public class TokensDbRepositoryTests
    {
        [Fact]
        public async Task AddAsync_WithCorrectToken()
        {
            Mock<PortfolioAnalyzerDb> _dbContextMock = new Mock<PortfolioAnalyzerDb>();
            Mock<DbSet<TinkoffToken>> _dbTokenSetMock = new Mock<DbSet<TinkoffToken>>();
            var token = new TinkoffToken() { Id = 0, Type = TokenType.Sandbox, Value = "TestTokenValue" };
            _dbContextMock.Setup(s => s.Set<TinkoffToken>()).Returns(_dbTokenSetMock.Object);
            _dbTokenSetMock.Setup(s => s.AddAsync(It.IsAny<TinkoffToken>(), It.IsAny<CancellationToken>()))
                .Returns(ValueTask.FromResult((EntityEntry<TinkoffToken>)null));

            var tokensDbRepo = new TokensDbRepository(_dbContextMock.Object);
            await tokensDbRepo.AddAsync(token);

            _dbContextMock.Verify(x => x.Set<TinkoffToken>(), Times.Once);
            _dbTokenSetMock.Verify(x => x.AddAsync(It.IsAny<TinkoffToken>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_WithCorrectToken()
        {
            Mock<PortfolioAnalyzerDb> _dbContextMock = new Mock<PortfolioAnalyzerDb>();
            Mock<DbSet<TinkoffToken>> _dbTokenSetMock = new Mock<DbSet<TinkoffToken>>();
            var token = new TinkoffToken() { Id = 0, Type = TokenType.Sandbox, Value = "TestTokenValue" };
            _dbContextMock.Setup(s => s.Set<TinkoffToken>()).Returns(_dbTokenSetMock.Object);
            _dbTokenSetMock.Setup(s => s.Remove(It.IsAny<TinkoffToken>()))
                .Returns((EntityEntry<TinkoffToken>)null);

            var tokensDbRepo = new TokensDbRepository(_dbContextMock.Object);
            await tokensDbRepo.RemoveAsync(token);

            _dbContextMock.Verify(x => x.Set<TinkoffToken>(), Times.Once);
            _dbTokenSetMock.Verify(x => x.Remove(It.IsAny<TinkoffToken>()), Times.Once);
        }
    }
}
