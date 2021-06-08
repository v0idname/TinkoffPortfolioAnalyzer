using Library.Commands;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Tinkoff.Trading.OpenApi.Models;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.ViewModels
{
    internal class AvailSecuritiesViewModel
    {
        //TODO: ObservableCollection ??
        public List<AvailSecSnapshot> AvailSecSnapshots { get; }

        public List<AvailSecSnapshot> SelectedAvailSecSnapshots { get; set; }

        public ICommand CreateSnapshotCommand { get; }

        private bool CanCreateSnapshotCommandExecute(object p) => true;

        private void OnCreateSnapshotCommandExecuted(object p)
        {
            //var newSnapshot = new AvailSecSnapshot
            //{
            //    CreatedDateTime = DateTime.Now,
            //    Securities = new List<SecurityInfo>()
            //};

            //var bonds = _curConnectContext.MarketBondsAsync().GetAwaiter().GetResult();
            //foreach (var bond in bonds.Instruments)
            //{
            //    newSnapshot.Securities.Add(new SecurityInfo
            //    {
            //        Name = bond.Name,
            //        Ticker = bond.Ticker,
            //        InstrumentType = bond.Type,
            //        Currency = bond.Currency
            //    });
            //}
            //AvailSecSnapshots.Add(newSnapshot);
        }

        public ICommand SelectedSnapChangedCommand { get; }

        private bool CanSelectedSnapChangedCommandExecute(object p) => true;

        private void OnSelectedSnapChangedCommandExecuted(object p)
        {
            ;
        }

        public AvailSecuritiesViewModel()
        {
            AvailSecSnapshots = new List<AvailSecSnapshot>();
            for (int snapIndex = 0; snapIndex < 3; snapIndex++)
            {
                var newSnapshot = new AvailSecSnapshot
                {
                    CreatedDateTime = new DateTime(DateTime.Now.Ticks).AddDays(snapIndex),
                    Securities = new List<SecurityInfo>()
                };
                for (int i = 0; i < 10 + snapIndex; i++)
                {
                    newSnapshot.Securities.Add(new SecurityInfo
                    {
                        Name = $"Security name {i}",
                        Ticker = $"Security ticker {i}",
                        InstrumentType = InstrumentType.Bond,
                        Currency = Currency.Rub
                    });
                }
                AvailSecSnapshots.Add(newSnapshot);
            }

            CreateSnapshotCommand = new RelayCommand(OnCreateSnapshotCommandExecuted, CanCreateSnapshotCommandExecute);
            SelectedSnapChangedCommand = new RelayCommand(OnSelectedSnapChangedCommandExecuted, CanSelectedSnapChangedCommandExecute);
            SelectedAvailSecSnapshots = new List<AvailSecSnapshot>();
        }
    }
}
