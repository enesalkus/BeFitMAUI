using BeFitMAUI.Views;

namespace BeFitMAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            Routing.RegisterRoute(nameof(AddSessionPage), typeof(AddSessionPage));
            Routing.RegisterRoute(nameof(SessionDetailPage), typeof(SessionDetailPage));
        }
    }
}
