using System;
using System.ComponentModel.DataAnnotations;
using Tinkoff.Trading.OpenApi.Models;

namespace TinkoffPortfolioAnalyzer.Models
{
    [Serializable]
    public class SecurityInfo : IEquatable<SecurityInfo>
    {
        public string Name { get; set; }

        [Key]
        public string Ticker { get; set; }

        public InstrumentType InstrumentType { get; set; }
        
        public Currency Currency { get; set; }

        public bool Equals(SecurityInfo other)
        {
            return Ticker == other?.Ticker;
        }

        public override int GetHashCode()
        {
            return string.GetHashCode(Ticker);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SecurityInfo);
        }
    }
}
