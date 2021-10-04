using System;

namespace TinkoffPortfolioAnalyzer.Models
{
    [Serializable]
    public class TinkoffToken
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Type} - {Value}";
        }
    }
}
