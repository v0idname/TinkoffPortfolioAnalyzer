﻿using System;
using Tinkoff.Trading.OpenApi.Models;

namespace TinkoffPortfolioAnalyzer.Models
{
    internal class SecurityInfo : IEquatable<SecurityInfo>
    {
        public string Name { get; set; }
        public string Ticker { get; set; }
        public InstrumentType InstrumentType { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }

        public bool Equals(SecurityInfo other)
        {
            return Ticker == other.Ticker;
        }

        public override int GetHashCode()
        {
            return string.GetHashCode(Ticker);
        }
    }
}
