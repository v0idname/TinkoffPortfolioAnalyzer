using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TinkoffPortfolioAnalyzer.Models
{
    [Serializable]
    internal class TinkoffTokensList
    {
        private ObservableCollection<TinkoffToken> _list = new();

        public ICollection<TinkoffToken> List => _list;
    }
}
