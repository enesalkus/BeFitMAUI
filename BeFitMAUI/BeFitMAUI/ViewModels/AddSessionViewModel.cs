using BeFitMAUI.Models;
using BeFitMAUI.Services;
using System.Windows.Input;

namespace BeFitMAUI.ViewModels
{
    public class AddSessionViewModel : BaseViewModel
    {
        private readonly TrainingService _trainingService;
        private readonly UserService _userService;
        private DateTime _startTime;
        private DateTime _endTime;

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

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddSessionViewModel(TrainingService trainingService, UserService userService)
        {
            _trainingService = trainingService;
            _userService = userService;
            StartTime = DateTime.Now;
            EndTime = DateTime.Now.AddHours(1);
            SaveCommand = new Command(async () => await SaveAsync());
            CancelCommand = new Command(async () => await CancelAsync());
        }

        private async Task SaveAsync()
        {
             string userId = Preferences.Get("UserId", string.Empty);
             if (string.IsNullOrEmpty(userId))
             {
                 userId = Guid.NewGuid().ToString();
                 Preferences.Set("UserId", userId);
             }

             // Ensure user exists in DB to prevent Foreign Key errors
             await _userService.GetOrCreateUserAsync(userId);

            var session = new TrainingSession
            {
                StartTime = StartTime,
                EndTime = EndTime,
                UserId = userId
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
