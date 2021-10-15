using Library.Commands;
using System.Windows;
using TinkoffPortfolioAnalyzer.Views.Windows;

namespace TinkoffPortfolioAnalyzer.Commands
{
    internal class ShowTokensWindowCommand : BaseCommand
    {
        private TokensManagementWindow _window;

        public override bool CanExecute(object parameter) => _window == null;

        public override void Execute(object parameter)
        {
            _window = new TokensManagementWindow
            {
                Owner = Application.Current.MainWindow
            };
            _window.ShowDialog();
            _window = null;
        }
    }
}
