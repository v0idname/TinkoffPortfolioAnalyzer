using Library.Commands;
using Library.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TinkoffPortfolioAnalyzer.Data.Repositories;
using TinkoffPortfolioAnalyzer.Models;
using TinkoffPortfolioAnalyzer.Services;

namespace TinkoffPortfolioAnalyzer.ViewModels
{
    internal class AvailSecuritiesViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly ISnapshotsRepository _snapService;

        public List<AvailSecSnapshot> SelectedAvailSecSnapshots { get; set; }

        public IEnumerable<AvailSecSnapshot> AvailSecSnapshots => _snapService.GetAll();

        private List<SecSnapshotDiff> _secSnapshotDiffs;
        public List<SecSnapshotDiff> SecSnapshotDiffs
        {
            get => _secSnapshotDiffs;
            set => Set(ref _secSnapshotDiffs, value);
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
            _snapService.Create(secList);
            OnPropertyChanged(nameof(AvailSecSnapshots));
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

        public ICommand DeleteSnapshotCommand { get; }

        private bool CanDeleteSnapshotCommandExecute(object p) => true;

        private void OnDeleteSnapshotCommandExecuted(object p)
        {
            _snapService.Remove((AvailSecSnapshot)p);
            OnPropertyChanged(nameof(AvailSecSnapshots));
        }

        public AvailSecuritiesViewModel(IDataService dataService, ISnapshotsRepository snapService)
        {
            _snapService = snapService;
            _dataService = dataService;
            CreateSnapshotCommand = new RelayCommand(OnCreateSnapshotCommandExecuted, CanCreateSnapshotCommandExecute);
            SelectedSnapChangedCommand = new RelayCommand(OnSelectedSnapChangedCommandExecuted, CanSelectedSnapChangedCommandExecute);
            DeleteSnapshotCommand = new RelayCommand(OnDeleteSnapshotCommandExecuted, CanDeleteSnapshotCommandExecute);
            SelectedAvailSecSnapshots = new List<AvailSecSnapshot>();
            OnPropertyChanged(nameof(AvailSecSnapshots));
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
                        IsSnap0Contains = i == 0,
                        IsSnap1Contains = i == 1
                    });
                }
            }

            return secSnapshotDiff;
        }
    }
}
