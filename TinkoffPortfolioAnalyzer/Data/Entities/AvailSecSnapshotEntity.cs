using Library.Data;
using System;
using System.Collections.Generic;

namespace TinkoffPortfolioAnalyzer.Data.Entities
{
    public class AvailSecSnapshotEntity : Entity
    {
        public DateTime CreatedDateTime { get; set; }
        public IEnumerable<SecurityInfoEntity> Securities { get; set; }
    }
}
