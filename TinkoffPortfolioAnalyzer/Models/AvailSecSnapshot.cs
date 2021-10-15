using Library.Data;
using System;
using System.Collections.Generic;

namespace TinkoffPortfolioAnalyzer.Models
{
    public class AvailSecSnapshot : Entity
    {
        public DateTime CreatedDateTime { get; set; }
        public IEnumerable<SecurityInfo> Securities { get; set; }
        //public new bool Equals(Entity other)
        //{
        //    return Id == other?.Id;
        //}

        //public override int GetHashCode()
        //{
        //    return Id.GetHashCode();
        //}

        //public override bool Equals(object obj)
        //{
        //    return Equals(obj as Entity);
        //}
    }
}
