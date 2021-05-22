using OxyPlot;
using OxyPlot.Series;

namespace TinkAnalyzerWpfDebug
{
    class MainWindowViewModel
    {
        public PlotModel plotModel = new PlotModel();

        public MainWindowViewModel()
        {
            var pieSeries = new PieSeries();
            for (int i = 0; i < 10; i++)
            {
                pieSeries.Slices.Add(new PieSlice($"slice {i}", i));
            }
            plotModel.Series.Add(pieSeries);
        }
    }
}
