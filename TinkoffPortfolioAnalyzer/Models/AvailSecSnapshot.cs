using System;
using System.Collections.Generic;

namespace TinkoffPortfolioAnalyzer.Models
{
    internal class AvailSecSnapshot
    {
        public DateTime CreatedDateTime { get; set; }
        public ICollection<SecurityInfo> Securities { get; set; }
    }
}
