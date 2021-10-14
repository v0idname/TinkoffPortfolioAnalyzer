using Library.Commands;
using Library.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TinkoffPortfolioAnalyzer.Data.Repositories;
using TinkoffPortfolioAnalyzer.Models;
using TinkoffPortfolioAnalyzer.Services;

namespace TinkoffPortfolioAnalyzer.ViewModels
{
    internal class AvailSecuritiesViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly ISnapshotsRepository _snapRepo;
        private readonly ISnapshotsComparerService _snapService;

        public IEnumerable<AvailSecSnapshot> SelectedAvailSecSnapshots { get; set; }

        public IEnumerable<AvailSecSnapshot> AvailSecSnapshots { get; private set; }

        private IEnumerable<SecSnapshotDiff> _secSnapshotDiffs;
        public IEnumerable<SecSnapshotDiff> SecSnapshotDiffs
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
            await _snapRepo.CreateAsync(secList);
            await UpdateSnapshots();
        }

        public ICommand SelectedSnapChangedCommand { get; }

        private bool CanSelectedSnapChangedCommandExecute(object p) => true;

        private void OnSelectedSnapChangedCommandExecuted(object p)
        {
            SecSnapshotDiffs = _snapService.Compare2Snapshots(SelectedAvailSecSnapshots);
            if (SelectedAvailSecSnapshots.Count() > 0)
                SelectedSnap0Name = SelectedAvailSecSnapshots.ElementAt(0).CreatedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            if (SelectedAvailSecSnapshots.Count() > 1)
                SelectedSnap1Name = SelectedAvailSecSnapshots.ElementAt(1).CreatedDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public ICommand DeleteSnapshotCommand { get; }

        private bool CanDeleteSnapshotCommandExecute(object p) => true;

        private async void OnDeleteSnapshotCommandExecuted(object p)
        {
            await _snapRepo.RemoveAsync((AvailSecSnapshot)p);
            await UpdateSnapshots();
        }

        public ICommand LoadedCommand { get; set; }

        private bool CanLoadedCommandExecute(object parameter) => AvailSecSnapshots == null;

        private async void OnLoadedCommandExecuted(object parameter)
        {
            await UpdateSnapshots();
        }

        public AvailSecuritiesViewModel(IDataService dataService, ISnapshotsRepository snapRepo, ISnapshotsComparerService snapService)
        {
            _snapRepo = snapRepo;
            _dataService = dataService;
            _snapService = snapService;
            CreateSnapshotCommand = new RelayCommand(OnCreateSnapshotCommandExecuted, CanCreateSnapshotCommandExecute);
            SelectedSnapChangedCommand = new RelayCommand(OnSelectedSnapChangedCommandExecuted, CanSelectedSnapChangedCommandExecute);
            DeleteSnapshotCommand = new RelayCommand(OnDeleteSnapshotCommandExecuted, CanDeleteSnapshotCommandExecute);
            LoadedCommand = new RelayCommand(OnLoadedCommandExecuted, CanLoadedCommandExecute);
            SelectedAvailSecSnapshots = new List<AvailSecSnapshot>();
        }

        private async Task UpdateSnapshots()
        {
            AvailSecSnapshots = await _snapRepo.GetAllAsync();
            OnPropertyChanged(nameof(AvailSecSnapshots));
        }
    }
}
