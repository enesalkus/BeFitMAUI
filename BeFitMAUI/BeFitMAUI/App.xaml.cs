using BeFitMAUI.Data;

namespace BeFitMAUI
{
    public partial class App : Application
    {
        public App(BeFitDbContext context)
        {
            InitializeComponent();
            
            context.Database.EnsureCreated();

            MainPage = new AppShell();
        }
    }
}