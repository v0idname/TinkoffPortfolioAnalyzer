using Library.Data;
using System;
using Tinkoff.Trading.OpenApi.Models;

namespace TinkoffPortfolioAnalyzer.Models
{
    [Serializable]
    public class SecurityInfo : Entity, IEquatable<SecurityInfo>
    {
        public string Name { get; set; }

        public string Ticker { get; set; }

        public InstrumentType InstrumentType { get; set; }
        
        public Currency Currency { get; set; }

        public bool Equals(SecurityInfo other)
        {
            return Ticker == other?.Ticker && base.Equals(other);
        }

        public override int GetHashCode()
        {
            return string.GetHashCode(Ticker);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SecurityInfo) && base.Equals(obj);
        }
    }
}
