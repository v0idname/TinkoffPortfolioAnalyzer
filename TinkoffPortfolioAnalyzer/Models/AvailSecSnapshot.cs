using Library.Data;
using System;
using System.Collections.Generic;

namespace TinkoffPortfolioAnalyzer.Models
{
    public class AvailSecSnapshot : Entity
    {
        public DateTime CreatedDateTime { get; set; }
        public IEnumerable<SecurityInfo> Securities { get; set; }
    }
}
