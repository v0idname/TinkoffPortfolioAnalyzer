using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Services
{
    internal class TokensXmlService : ITokensService
    {
        private TinkoffTokensList _tokens = new();
        private const string TokensFileName = "./tokens.xml";
        XmlSerializer _xmlFormatter = new XmlSerializer(typeof(TinkoffTokensList));

        public TokensXmlService()
        {
            if (!File.Exists(TokensFileName))
            {
                File.Create(TokensFileName);
                return;
            }

            using (var stream = File.OpenRead(TokensFileName))
            {
                if (stream.Length == 0) return;

                var tokensList = (TinkoffTokensList)_xmlFormatter.Deserialize(stream);
                foreach (var token in tokensList.List)
                {
                    _tokens.List.Add(token);
                }
            }
        }

        public void AddToken(TinkoffToken tokenToAdd)
        {
            if (_tokens.List.Contains(tokenToAdd))
                return;

            _tokens.List.Add(tokenToAdd);
            using (var stream = File.OpenWrite(TokensFileName))
            {
                _xmlFormatter.Serialize(stream, _tokens);
            }
        }

        public void DeleteToken(TinkoffToken tokenToDelete)
        {
            _tokens.List.Remove(tokenToDelete);
            using (var stream = File.OpenWrite(TokensFileName))
            {
                _xmlFormatter.Serialize(stream, _tokens);
            }
        }

        public IEnumerable<TinkoffToken> GetTokens() => _tokens.List;
    }
}
