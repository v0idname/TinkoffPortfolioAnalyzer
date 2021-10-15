using Library.Data;
using System.Collections.Generic;
using Tinkoff.Trading.OpenApi.Models;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Data.Entities
{
    public class SecurityInfoEntity : Entity
    {
        public string Name { get; set; }

        public string Ticker { get; set; }

        public InstrumentType InstrumentType { get; set; }

        public Currency Currency { get; set; }

        public IEnumerable<AvailSecSnapshot> Snapshots { get; set; }
    }
}
