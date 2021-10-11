using System;
using System.Collections.ObjectModel;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Data
{
    [Serializable]
    public class TinkoffTokensList
    {
        private ObservableCollection<TinkoffToken> _list = new();

        public ObservableCollection<TinkoffToken> List => _list;
    }
}
