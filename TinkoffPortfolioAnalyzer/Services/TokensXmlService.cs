using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Services
{
    internal class TokensXmlService : ITokensService
    {
        private ICollection<TinkoffToken> _tokens = new TinkoffTokensList().List;
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
                var tokensList = (TinkoffTokensList)_xmlFormatter.Deserialize(stream);
                foreach (var token in tokensList.List)
                {
                    _tokens.Add(token);
                }
            }
        }

        public void AddToken(TinkoffToken tokenToAdd)
        {
            if (_tokens.Contains(tokenToAdd))
                return;

            _tokens.Add(tokenToAdd);
            using (var stream = File.OpenWrite(TokensFileName))
            {
                _xmlFormatter.Serialize(stream, _tokens);
            }
        }

        public void DeleteToken(TinkoffToken tokenToDelete)
        {
            _tokens.Remove(tokenToDelete);
            using (var stream = File.OpenWrite(TokensFileName))
            {
                _xmlFormatter.Serialize(stream, _tokens);
            }
        }

        public IEnumerable<TinkoffToken> GetTokens() => _tokens;
    }
}
