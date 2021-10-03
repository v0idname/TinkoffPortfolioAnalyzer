using System.Collections;
using System.Windows.Controls;

namespace TinkoffPortfolioAnalyzer.ViewModels
{
    public class SelectedItemsBehavior
    {
        private readonly ListBox _listBox;
        private readonly IList _boundList;

        public SelectedItemsBehavior(ListBox listBox, IList boundList)
        {
            _boundList = boundList;
            _listBox = listBox;
            _listBox.SelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _boundList.Clear();

            foreach (var item in _listBox.SelectedItems)
            {
                _boundList.Add(item);
            }
        }
    }
}
