using Library.Data;
using System;

namespace TinkoffPortfolioAnalyzer.Models
{
    [Serializable]
    public class TinkoffToken : Entity
    {
        public string Value { get; set; }
        public TokenType Type { get; set; }
        
        public override string ToString()
        {
            return $"{Type} - {Value}";
        }
    }
}
