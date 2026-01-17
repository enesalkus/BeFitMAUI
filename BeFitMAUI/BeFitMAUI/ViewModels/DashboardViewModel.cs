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
                // TODO: Get userId from a UserService or settings. Using a placeholder for now.
                // Assuming single user per device for now, or fetch the user created in App.xaml.cs?
                // For now, we will assume we can get one user or just pass a known ID if we seed one.
                // Or better, StatsService could assume default user if userId is null, but we implemented with userId.
                 
                // We'll query all users and take first for now or null
                // Wait, we need a user id. Let's hardcode one for testing or fetch context.
                // Ideally, we should have a generic "User" context service.
                // I will modify StatsService usage to just pass a hardcoded ID for now since we haven't implemented Login.
                
                // Let's create a temporary user ID management strategy. 
                // Creating a new GUID for every session is bad. 
                // We should store it in Preferences.
                
                string userId = Preferences.Get("UserId", string.Empty);
                if (string.IsNullOrEmpty(userId))
                {
                    userId = Guid.NewGuid().ToString();
                    Preferences.Set("UserId", userId);
                }

                var statsList = await _statsService.GetStatsAsync(userId);
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
