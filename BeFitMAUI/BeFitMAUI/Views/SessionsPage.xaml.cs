using BeFitMAUI.ViewModels;

namespace BeFitMAUI.Views
{
    public partial class SessionsPage : ContentPage
    {
        private readonly SessionsViewModel _viewModel;

        public SessionsPage(SessionsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadSessionsAsync();
        }
    }
}
