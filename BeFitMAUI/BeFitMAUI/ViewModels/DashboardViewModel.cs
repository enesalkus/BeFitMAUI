using BeFitMAUI.Models;
using BeFitMAUI.Services;
using System.Collections.ObjectModel;

namespace BeFitMAUI.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly StatsService _statsService;
        private ObservableCollection<ExerciseStatisticsViewModel> _stats;
        private bool _isLoading;

        public ObservableCollection<ExerciseStatisticsViewModel> Stats
        {
            get => _stats;
            set => SetProperty(ref _stats, value);
        }
        
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public DashboardViewModel(StatsService statsService)
        {
            _statsService = statsService;
            Stats = new ObservableCollection<ExerciseStatisticsViewModel>();
        }

        public async Task LoadStatsAsync()
        {
            if (IsLoading) return;

            IsLoading = true;
            try
            {
                var statsList = await _statsService.GetStatsAsync();
                Stats.Clear();
                foreach (var s in statsList)
                {
                    Stats.Add(s);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
