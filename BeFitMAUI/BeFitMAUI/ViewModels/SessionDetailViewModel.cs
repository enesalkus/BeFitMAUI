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
        public ICommand EditSessionCommand { get; }
        public ICommand AddExerciseCommand { get; }
        public ICommand EditExerciseCommand { get; }
        public ICommand DeleteExerciseCommand { get; }

        public SessionDetailViewModel(TrainingService trainingService, ExerciseService exerciseService)
        {
            _trainingService = trainingService;
            _exerciseService = exerciseService;
            LoadSessionCommand = new Command(async () => await LoadSessionAsync());
            DeleteSessionCommand = new Command(async () => await DeleteSessionAsync());
            EditSessionCommand = new Command(async () => await EditSessionAsync());
            AddExerciseCommand = new Command(async () => await AddExerciseAsync());
            EditExerciseCommand = new Command<ExercisePerformed>(async (ep) => await EditExerciseAsync(ep));
            DeleteExerciseCommand = new Command<ExercisePerformed>(async (ep) => await DeleteExerciseAsync(ep));
        }

        private async Task LoadSessionAsync()
        {
            IsLoading = true;
            try
            {
                Session = await _trainingService.GetSessionAsync(SessionId);
            }
            finally
            {
                IsLoading = false;
            }
        }
        
        private async Task EditSessionAsync()
        {
             await Shell.Current.GoToAsync($"AddSessionPage?Id={SessionId}");
        }

        private async Task DeleteSessionAsync()
        {
            bool answer = await Shell.Current.DisplayAlert("Usuń", "Czy na pewno usunąć ten trening?", "Tak", "Nie");
            if (answer)
            {
                await _trainingService.DeleteSessionAsync(Session);
                await Shell.Current.GoToAsync("..");
            }
        }

        private async Task AddExerciseAsync()
        {
            var exercises = await _exerciseService.GetExerciseTypesAsync();
            var exerciseNames = exercises.Select(e => e.Name).ToArray();
            
            string action = await Shell.Current.DisplayActionSheet("Wybierz ćwiczenie", "Anuluj", null, exerciseNames);
            if (action != "Anuluj" && action != null)
            {
                var selectedType = exercises.FirstOrDefault(e => e.Name == action);
                if (selectedType != null)
                {
                   string loadStr = await Shell.Current.DisplayPromptAsync("Obciążenie", "Podaj obciążenie (kg):", initialValue: "0", keyboard: Keyboard.Numeric);
                   string setsStr = await Shell.Current.DisplayPromptAsync("Serie", "Podaj liczbę serii:", initialValue: "3", keyboard: Keyboard.Numeric);
                   string repsStr = await Shell.Current.DisplayPromptAsync("Powtórzenia", "Podaj liczbę powtórzeń:", initialValue: "10", keyboard: Keyboard.Numeric);

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
        
        private async Task EditExerciseAsync(ExercisePerformed ep)
        {
            if (ep == null) return;
            
           string loadStr = await Shell.Current.DisplayPromptAsync("Edytuj Obciążenie", "Nowe obciążenie (kg):", initialValue: ep.Load.ToString(), keyboard: Keyboard.Numeric);
           string setsStr = await Shell.Current.DisplayPromptAsync("Edytuj Serie", "Nowa liczba serii:", initialValue: ep.Sets.ToString(), keyboard: Keyboard.Numeric);
           string repsStr = await Shell.Current.DisplayPromptAsync("Edytuj Powtórzenia", "Nowa liczba powtórzeń:", initialValue: ep.Repetitions.ToString(), keyboard: Keyboard.Numeric);

           if (double.TryParse(loadStr, out double load) && int.TryParse(setsStr, out int sets) && int.TryParse(repsStr, out int reps))
           {
                ep.Load = load;
                ep.Sets = sets;
                ep.Repetitions = reps;
                
                await _trainingService.SaveExerciseAsync(ep);
                await LoadSessionAsync();
           }
        }

        private async Task DeleteExerciseAsync(ExercisePerformed ep)
        {
            bool answer = await Shell.Current.DisplayAlert("Usuń", "Usunąć ćwiczenie?", "Tak", "Nie");
            if (answer)
            {
                await _trainingService.DeleteExerciseAsync(ep);
                await LoadSessionAsync();
            }
        }
    }
}
