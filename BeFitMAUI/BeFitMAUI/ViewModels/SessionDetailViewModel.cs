using BeFitMAUI.Models;
using BeFitMAUI.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BeFitMAUI.ViewModels
{
    [QueryProperty(nameof(SessionId), "Id")]
    public class SessionDetailViewModel : BaseViewModel
    {
        private readonly TrainingService _trainingService;
        private readonly ExerciseService _exerciseService;
        private int _sessionId;
        private TrainingSession _session;
        private bool _isLoading;

        public int SessionId
        {
            get => _sessionId;
            set
            {
                _sessionId = value;
                LoadSessionAsync();
            }
        }

        public TrainingSession Session
        {
            get => _session;
            set => SetProperty(ref _session, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand LoadSessionCommand { get; }
        public ICommand DeleteSessionCommand { get; }
        public ICommand AddExerciseCommand { get; }
        public ICommand DeleteExerciseCommand { get; }

        public SessionDetailViewModel(TrainingService trainingService, ExerciseService exerciseService)
        {
            _trainingService = trainingService;
            _exerciseService = exerciseService;
            LoadSessionCommand = new Command(async () => await LoadSessionAsync());
            DeleteSessionCommand = new Command(async () => await DeleteSessionAsync());
            AddExerciseCommand = new Command(async () => await AddExerciseAsync());
            DeleteExerciseCommand = new Command<ExercisePerformed>(async (ep) => await DeleteExerciseAsync(ep));
        }

        private async Task LoadSessionAsync()
        {
            IsLoading = true;
            try
            {
                string userId = Preferences.Get("UserId", string.Empty);
                Session = await _trainingService.GetSessionAsync(SessionId, userId);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteSessionAsync()
        {
            bool answer = await Shell.Current.DisplayAlert("Delete", "Delete this session?", "Yes", "No");
            if (answer)
            {
                await _trainingService.DeleteSessionAsync(Session);
                await Shell.Current.GoToAsync("..");
            }
        }

        private async Task AddExerciseAsync()
        {
            // Simple approach: DisplayActionSheet with Exercises or navigate to a selection page.
            // For now, let's just pick from a list via ActionSheet for simplicity if list is small, 
            // but normally we need a dedicated page.
            var exercises = await _exerciseService.GetExerciseTypesAsync();
            var exerciseNames = exercises.Select(e => e.Name).ToArray();
            
            string action = await Shell.Current.DisplayActionSheet("Select Exercise", "Cancel", null, exerciseNames);
            if (action != "Cancel" && action != null)
            {
                var selectedType = exercises.FirstOrDefault(e => e.Name == action);
                if (selectedType != null)
                {
                   // Prompt for Load, Sets, Reps -> This is clumsy with prompts.
                   // Better: Navigate to AddExercisePerformedPage passing SessionId and ExerciseTypeId.
                   // But to be quick, I'll use prompts or just add default and let user edit (if we add edit).
                   // Let's use prompts for now.
                   
                   string loadStr = await Shell.Current.DisplayPromptAsync("Load", "Enter Load (kg):", keyboard: Keyboard.Numeric);
                   string setsStr = await Shell.Current.DisplayPromptAsync("Sets", "Enter Sets:", keyboard: Keyboard.Numeric);
                   string repsStr = await Shell.Current.DisplayPromptAsync("Reps", "Enter Repetitions:", keyboard: Keyboard.Numeric);

                   if (double.TryParse(loadStr, out double load) && int.TryParse(setsStr, out int sets) && int.TryParse(repsStr, out int reps))
                   {
                        var ep = new ExercisePerformed
                        {
                            TrainingSessionId = SessionId,
                            ExerciseTypeId = selectedType.Id,
                            Load = load,
                            Sets = sets,
                            Repetitions = reps
                        };
                        await _trainingService.SaveExerciseAsync(ep);
                        await LoadSessionAsync();
                   }
                }
            }
        }

        private async Task DeleteExerciseAsync(ExercisePerformed ep)
        {
            bool answer = await Shell.Current.DisplayAlert("Delete", "Remove exercise?", "Yes", "No");
            if (answer)
            {
                await _trainingService.DeleteExerciseAsync(ep);
                await LoadSessionAsync();
            }
        }
    }
}
