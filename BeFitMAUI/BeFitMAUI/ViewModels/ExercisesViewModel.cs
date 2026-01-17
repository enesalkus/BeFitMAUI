using BeFitMAUI.Models;
using BeFitMAUI.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BeFitMAUI.ViewModels
{
    public class ExercisesViewModel : BaseViewModel
    {
        private readonly ExerciseService _exerciseService;
        private ObservableCollection<ExerciseType> _exercises;
        private bool _isLoading;

        public ObservableCollection<ExerciseType> Exercises
        {
            get => _exercises;
            set => SetProperty(ref _exercises, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }
        
        public ICommand LoadExercisesCommand { get; }
        public ICommand AddExerciseCommand { get; }
        public ICommand EditExerciseCommand { get; }
        public ICommand DeleteExerciseCommand { get; }

        public ExercisesViewModel(ExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
            Exercises = new ObservableCollection<ExerciseType>();
            LoadExercisesCommand = new Command(async () => await LoadExercisesAsync());
            AddExerciseCommand = new Command(async () => await AddExerciseAsync());
            EditExerciseCommand = new Command<ExerciseType>(async (e) => await EditExerciseAsync(e));
            DeleteExerciseCommand = new Command<ExerciseType>(async (e) => await DeleteExerciseAsync(e));
        }

        public async Task LoadExercisesAsync()
        {
            if (IsLoading) return;
            IsLoading = true;
            try
            {
                var list = await _exerciseService.GetExerciseTypesAsync();
                Exercises.Clear();
                foreach (var item in list)
                {
                    Exercises.Add(item);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task AddExerciseAsync()
        {
            string result = await Shell.Current.DisplayPromptAsync("Nowe ćwiczenie", "Podaj nazwę ćwiczenia:", "OK", "Anuluj");
            if (!string.IsNullOrWhiteSpace(result))
            {
                var newExercise = new ExerciseType { Name = result };
                await _exerciseService.SaveExerciseTypeAsync(newExercise);
                await LoadExercisesAsync();
            }
        }
        
        private async Task EditExerciseAsync(ExerciseType exercise)
        {
            if (exercise == null) return;
            string result = await Shell.Current.DisplayPromptAsync("Edytuj ćwiczenie", "Zmień nazwę:", "Zapisz", "Anuluj", initialValue: exercise.Name);
            if (!string.IsNullOrWhiteSpace(result))
            {
                exercise.Name = result;
                await _exerciseService.SaveExerciseTypeAsync(exercise);
                await LoadExercisesAsync();
            }
        }
        
        private async Task DeleteExerciseAsync(ExerciseType exercise)
        {
            if (exercise == null) return;
            bool answer = await Shell.Current.DisplayAlert("Usuń", $"Czy na pewno usunąć {exercise.Name}?", "Tak", "Nie");
            if (answer)
            {
                await _exerciseService.DeleteExerciseTypeAsync(exercise);
                await LoadExercisesAsync();
            }
        }
    }
}
