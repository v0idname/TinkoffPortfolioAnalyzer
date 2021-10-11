using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Data
{
    internal class TokensXmlRepository : ITokensRepository
    {
        private TinkoffTokensList _tokens = new();
        private const string TokensFileName = "./tokens.xml";
        XmlSerializer _xmlFormatter = new XmlSerializer(typeof(TinkoffTokensList));

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

        public void Add(TinkoffToken tokenToAdd)
        {
            if (_tokens.List.Contains(tokenToAdd))
                return;

            _tokens.List.Add(tokenToAdd);
            File.WriteAllText(TokensFileName, string.Empty);
            using (var stream = File.OpenWrite(TokensFileName))
            {
                _xmlFormatter.Serialize(stream, _tokens);
            }
        }

        public void Remove(TinkoffToken tokenToDelete)
        {
            _tokens.List.Remove(tokenToDelete);
            using (var stream = File.OpenWrite(TokensFileName))
            {
                _xmlFormatter.Serialize(stream, _tokens);
            }
        }

        public IEnumerable<TinkoffToken> GetAll() => _tokens.List;
    }
}
