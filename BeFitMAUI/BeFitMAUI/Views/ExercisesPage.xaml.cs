using BeFitMAUI.ViewModels;

namespace BeFitMAUI.Views
{
    public partial class ExercisesPage : ContentPage
    {
        private readonly ExercisesViewModel _viewModel;

        public ExercisesPage(ExercisesViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadExercisesAsync();
        }
    }
}
