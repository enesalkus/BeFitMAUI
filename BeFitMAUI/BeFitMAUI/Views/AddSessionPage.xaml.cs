using BeFitMAUI.ViewModels;

namespace BeFitMAUI.Views
{
    public partial class AddSessionPage : ContentPage
    {
        public AddSessionPage(AddSessionViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
