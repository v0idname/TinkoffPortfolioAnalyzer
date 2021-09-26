using Microsoft.Extensions.DependencyInjection;

namespace TinkoffPortfolioAnalyzer.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowViewModel => App.Host.Services.GetRequiredService<MainWindowViewModel>();
        public AvailSecuritiesViewModel AvailSecuritiesViewModel => App.Host.Services.GetRequiredService<AvailSecuritiesViewModel>();
    }
}
