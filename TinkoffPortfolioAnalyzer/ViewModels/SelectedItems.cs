using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace TinkoffPortfolioAnalyzer.ViewModels
{
    public static class SelectedItems
    {
        private static readonly DependencyProperty SelectedItemsBehaviorProperty =
            DependencyProperty.RegisterAttached(
                "SelectedItemsBehavior",
                typeof(SelectedItemsBehavior),
                typeof(ListBox),
                null);

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.RegisterAttached(
                "Items",
                typeof(IList),
                typeof(SelectedItems),
                new PropertyMetadata(null, ItemsPropertyChanged));

        public static void SetItems(ListBox listBox, IList list) { listBox.SetValue(ItemsProperty, list); }
        public static IList GetItems(ListBox listBox) { return listBox.GetValue(ItemsProperty) as IList; }

        private static void ItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as ListBox;
            if (target != null)
            {
                GetOrCreateBehavior(target, e.NewValue as IList);
            }
        }

        private static SelectedItemsBehavior GetOrCreateBehavior(ListBox target, IList list)
        {
            var behavior = target.GetValue(SelectedItemsBehaviorProperty) as SelectedItemsBehavior;
            if (behavior == null)
            {
                behavior = new SelectedItemsBehavior(target, list);
                target.SetValue(SelectedItemsBehaviorProperty, behavior);
            }

            return behavior;
        }
    }
}
