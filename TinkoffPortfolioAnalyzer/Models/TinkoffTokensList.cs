using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TinkoffPortfolioAnalyzer.Models
{
    [Serializable]
    public class TinkoffTokensList
    {
        private ObservableCollection<TinkoffToken> _list = new();

        public ObservableCollection<TinkoffToken> List => _list;
    }
}
