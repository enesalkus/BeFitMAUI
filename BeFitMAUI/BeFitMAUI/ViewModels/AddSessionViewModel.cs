using BeFitMAUI.Models;
using BeFitMAUI.Services;
using System.Windows.Input;

namespace BeFitMAUI.ViewModels
{
    [QueryProperty(nameof(SessionId), "Id")]
    public class AddSessionViewModel : BaseViewModel
    {
        private readonly TrainingService _trainingService;
        private DateTime _startTime;
        private DateTime _endTime;
        private int _sessionId;
        private bool _isEditMode;

        public int SessionId
        {
            get => _sessionId;
            set
            {
                _sessionId = value;
                if (value > 0)
                {
                    LoadSessionAsync(value);
                }
            }
        }

        public DateTime StartTime
        {
            get => _startTime;
            set => SetProperty(ref _startTime, value);
        }

        public DateTime EndTime
        {
            get => _endTime;
            set => SetProperty(ref _endTime, value);
        }
        
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddSessionViewModel(TrainingService trainingService)
        {
            _trainingService = trainingService;
            StartTime = DateTime.Now;
            EndTime = DateTime.Now.AddHours(1);
            SaveCommand = new Command(async () => await SaveAsync());
            CancelCommand = new Command(async () => await CancelAsync());
        }

        private async Task LoadSessionAsync(int id)
        {
            var session = await _trainingService.GetSessionAsync(id);
            if (session != null)
            {
                StartTime = session.StartTime;
                EndTime = session.EndTime;
                IsEditMode = true;
            }
        }

        private async Task SaveAsync()
        {
            var session = new TrainingSession
            {
                Id = SessionId,
                StartTime = StartTime,
                EndTime = EndTime
            };

            await _trainingService.SaveSessionAsync(session);
            await Shell.Current.GoToAsync("..");
        }

        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
