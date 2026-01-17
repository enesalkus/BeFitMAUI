using BeFitMAUI.Models;
using BeFitMAUI.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BeFitMAUI.ViewModels
{
    public class SessionsViewModel : BaseViewModel
    {
        private readonly TrainingService _trainingService;
        private ObservableCollection<TrainingSession> _sessions;
        private bool _isLoading;

        public ObservableCollection<TrainingSession> Sessions
        {
            get => _sessions;
            set => SetProperty(ref _sessions, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand LoadSessionsCommand { get; }
        public ICommand AddSessionCommand { get; }
        public ICommand EditSessionCommand { get; }
        public ICommand SessionSelectedCommand { get; }

        public SessionsViewModel(TrainingService trainingService)
        {
            _trainingService = trainingService;
            Sessions = new ObservableCollection<TrainingSession>();
            LoadSessionsCommand = new Command(async () => await LoadSessionsAsync());
            AddSessionCommand = new Command(async () => await AddSessionAsync());
            EditSessionCommand = new Command<TrainingSession>(async (s) => await EditSessionAsync(s));
            SessionSelectedCommand = new Command<TrainingSession>(async (session) => await OnSessionSelected(session));
        }

        public async Task LoadSessionsAsync()
        {
            if (IsLoading) return;
            IsLoading = true;
            try
            {
                var sessions = await _trainingService.GetSessionsAsync();
                Sessions.Clear();
                foreach (var session in sessions)
                {
                    Sessions.Add(session);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task AddSessionAsync()
        {
             await Shell.Current.GoToAsync("AddSessionPage");
        }

        private async Task EditSessionAsync(TrainingSession session)
        {
            if (session == null) return;
            await Shell.Current.GoToAsync($"AddSessionPage?Id={session.Id}");
        }

        private async Task OnSessionSelected(TrainingSession session)
        {
            if (session == null) return;
             await Shell.Current.GoToAsync($"SessionDetailPage?Id={session.Id}");
        }
    }
}
