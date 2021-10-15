using Library.Data;
using System;
using System.Collections.Generic;
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

        public IEnumerable<AvailSecSnapshot> Snapshots { get; set; }

        public bool Equals(SecurityInfo other)
        {
            if (other == null)
                return false;

            return Name == other.Name
                && Ticker == other.Ticker
                && InstrumentType == other.InstrumentType
                && Currency == other.Currency;
                //&& base.Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Ticker, InstrumentType, Currency);
            //return HashCode.Combine(Ticker, InstrumentType, Currency, base.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SecurityInfo);
            //return Equals(obj as SecurityInfo) && base.Equals(obj);
        }
    }
}
