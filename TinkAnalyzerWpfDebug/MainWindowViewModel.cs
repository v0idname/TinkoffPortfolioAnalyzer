using Library.ViewModels;
using OxyPlot;
using OxyPlot.Series;

namespace TinkAnalyzerWpfDebug
{
    class MainWindowViewModel : BaseViewModel
    {
        private PlotModel _plotModel = new PlotModel();
        public PlotModel PlotModel 
        {
            get => _plotModel;
            set => Set(ref _plotModel, value); 
        }

        public MainWindowViewModel()
        {
            var pieSeries = new PieSeries();
            for (int i = 0; i < 10; i++)
            {
                pieSeries.Slices.Add(new PieSlice($"slice {i}", i));
            }
            _plotModel.Series.Add(pieSeries);
        }
    }
}
