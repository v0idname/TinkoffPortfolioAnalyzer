using Library.ViewModels;
using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Linq;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.ViewModels
{
    internal class PortfolioPlotViewModel : BaseViewModel
    {
        private PlotModel _secStats;
        
        /// <summary>
        /// Доступные аккаунты для выбранного токена.
        /// </summary>
        public PlotModel SecuritiesPlotModel
        {
            get => _secStats;
            set => Set(ref _secStats, value);
        }

        private string _debugString;
        public string DebugString
        {
            get => _debugString;
            set => Set(ref _debugString, value);
        }

        private PlotModel GetSecuritiesPlotModel(IEnumerable<PortfolioSecurityInfo> securitiesInfo)
        {
            var plotModel = new PlotModel();
            if (securitiesInfo == null)
                return plotModel;

            var pieSeries = new PieSeries()
            {
                InsideLabelFormat = "",
                OutsideLabelFormat = "{1}: {2:0.0}%",
            };

            foreach (var security in securitiesInfo.OrderBy(x => x.TotalPrice))
            {
                pieSeries.Slices.Add(new PieSlice(security.Name, decimal.ToDouble(security.TotalPrice)));
            }
            plotModel.Series.Add(pieSeries);
            return plotModel;
        }

        public void RefreshSecuritiesPlotModel(IEnumerable<PortfolioSecurityInfo> securitiesInfo)
        {
            //Debug.WriteLine($"RefreshSecuritiesPlotModel()");
            SecuritiesPlotModel = GetSecuritiesPlotModel(securitiesInfo);
        }
    }
}
