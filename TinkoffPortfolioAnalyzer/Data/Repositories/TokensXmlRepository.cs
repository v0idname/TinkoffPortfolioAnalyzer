using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Data.Repositories
{
    internal class TokensXmlRepository : ITokensRepository
    {
        private TinkoffTokensList _tokens = new();
        private const string TokensFileName = "./tokens.xml";
        XmlSerializer _xmlFormatter = new XmlSerializer(typeof(TinkoffTokensList));

        public event EventHandler<EventArgs> RepositoryChanged;

        public TokensXmlRepository()
        {
            if (!File.Exists(TokensFileName))
            {
                File.Create(TokensFileName);
                return;
            }

            using (var stream = File.OpenRead(TokensFileName))
            {
                if (stream.Length == 0) return;

                try
                {
                    var tokensList = (TinkoffTokensList)_xmlFormatter.Deserialize(stream);
                    foreach (var token in tokensList.List)
                    {
                        _tokens.List.Add(token);
                    }
                }
                catch (InvalidOperationException)
                {
                    // Ошибка при чтении xml файла с токенами
                }
            }
        }

        public async Task AddAsync(TinkoffToken tokenToAdd)
        {
            if (_tokens.List.Contains(tokenToAdd))
                return;

            _tokens.List.Add(tokenToAdd);
            await Task.Run(() =>
            {
                File.WriteAllText(TokensFileName, string.Empty);
                using var stream = File.OpenWrite(TokensFileName);
                _xmlFormatter.Serialize(stream, _tokens);
            });
            RepositoryChanged?.Invoke(this, new EventArgs());
        }

        public async Task RemoveAsync(TinkoffToken tokenToDelete)
        {
            _tokens.List.Remove(tokenToDelete);
            await Task.Run(() =>
            {
                using var stream = File.OpenWrite(TokensFileName);
                _xmlFormatter.Serialize(stream, _tokens);
            });
            RepositoryChanged?.Invoke(this, new EventArgs());
        }

        public async Task<IEnumerable<TinkoffToken>> GetAllAsync()
        {
            return await Task.FromResult(_tokens.List);
        }
    }
}
