using BeFitMAUI.ViewModels;

namespace BeFitMAUI.Views
{
    public partial class SessionDetailPage : ContentPage
    {
        public SessionDetailPage(SessionDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
