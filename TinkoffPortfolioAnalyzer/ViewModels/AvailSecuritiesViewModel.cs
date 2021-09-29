using Library.Commands;
using Library.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TinkoffPortfolioAnalyzer.Models;
using TinkoffPortfolioAnalyzer.Services;

namespace TinkoffPortfolioAnalyzer.ViewModels
{
    internal class AvailSecuritiesViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly ISnapshotService _snapService;

        public List<AvailSecSnapshot> SelectedAvailSecSnapshots { get; set; }

        private IEnumerable<AvailSecSnapshot> _availSecSnapshots;
        public IEnumerable<AvailSecSnapshot> AvailSecSnapshots
        {
            get
            {
                return _availSecSnapshots;
            }
            set
            {
                Set(ref _availSecSnapshots, value);
            }
        }

        private List<SecSnapshotDiff> _secSnapshotDiffs;
        public List<SecSnapshotDiff> SecSnapshotDiffs
        {
            get
            {
                return _secSnapshotDiffs;
            }
            set
            {
                Set(ref _secSnapshotDiffs, value);
            }
        }

        public string _selectedSnap0Name;
        public string SelectedSnap0Name 
        { 
            get => _selectedSnap0Name;
            set => Set(ref _selectedSnap0Name, value);
        }

        public string _selectedSnap1Name;
        

        public string SelectedSnap1Name
        {
            get => _selectedSnap1Name;
            set => Set(ref _selectedSnap1Name, value);
        }

        public ICommand CreateSnapshotCommand { get; }

        private bool CanCreateSnapshotCommandExecute(object p) => true;

        private async void OnCreateSnapshotCommandExecuted(object p)
        {
            var secList = await _dataService.GetMarketSecuritiesAsync();
            _snapService.CreateSnapshot(secList);
            AvailSecSnapshots = _snapService.GetSnapshots();
        }

        public ICommand SelectedSnapChangedCommand { get; }

        private bool CanSelectedSnapChangedCommandExecute(object p) => true;

        private void OnSelectedSnapChangedCommandExecuted(object p)
        {
            SecSnapshotDiffs = GetSecSnapshotDiffs(SelectedAvailSecSnapshots);
            if (SelectedAvailSecSnapshots.Count > 0)
                SelectedSnap0Name = SelectedAvailSecSnapshots[0].CreatedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (SelectedAvailSecSnapshots.Count > 1)
                SelectedSnap1Name = SelectedAvailSecSnapshots[1].CreatedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public AvailSecuritiesViewModel(IDataService dataService, ISnapshotService snapService)
        {
            //AvailSecSnapshots = new List<AvailSecSnapshot>();
            //for (int snapIndex = 0; snapIndex < 3; snapIndex++)
            //{
            //    var newSnapshot = new AvailSecSnapshot
            //    {
            //        CreatedDateTime = new DateTime(DateTime.Now.Ticks).AddDays(snapIndex),
            //        Securities = new List<SecurityInfo>()
            //    };
            //    for (int i = 0; i < 10 + snapIndex; i++)
            //    {
            //        newSnapshot.Securities.Add(new SecurityInfo
            //        {
            //            Name = $"Security name {i}",
            //            Ticker = $"Security ticker {i}",
            //            InstrumentType = InstrumentType.Bond,
            //            Currency = Currency.Rub
            //        });
            //    }
            //    AvailSecSnapshots.Add(newSnapshot);
            //}

            _snapService = snapService;
            _dataService = dataService;
            CreateSnapshotCommand = new RelayCommand(OnCreateSnapshotCommandExecuted, CanCreateSnapshotCommandExecute);
            SelectedSnapChangedCommand = new RelayCommand(OnSelectedSnapChangedCommandExecuted, CanSelectedSnapChangedCommandExecute);
            SelectedAvailSecSnapshots = new List<AvailSecSnapshot>();
            AvailSecSnapshots = _snapService.GetSnapshots();
        }

        private List<SecSnapshotDiff> GetSecSnapshotDiffs(List<AvailSecSnapshot> snaps)
        {
            var secSnapshotDiff = new List<SecSnapshotDiff>();

            if (snaps.Count != 2)
                return secSnapshotDiff;

            var exclusiveSecurities = new List<List<SecurityInfo>>(2);
            exclusiveSecurities.Add(snaps[0].Securities.Except(snaps[1].Securities).ToList());
            exclusiveSecurities.Add(snaps[1].Securities.Except(snaps[0].Securities).ToList());

            for (int i = 0; i < exclusiveSecurities.Count; i++)
            {
                for (int j = 0; j < exclusiveSecurities[i].Count; j++)
                {
                    secSnapshotDiff.Add(new SecSnapshotDiff()
                    {
                        Ticker = exclusiveSecurities[i][j].Ticker,
                        Name = exclusiveSecurities[i][j].Name,
                        IsSnap0Contains = i == 0 ? true : false,
                        IsSnap1Contains = i == 1 ? true : false
                    });
                }
            }

            return secSnapshotDiff;
        }
    }
}
