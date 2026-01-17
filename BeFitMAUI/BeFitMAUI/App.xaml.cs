using BeFitMAUI.Data;

namespace BeFitMAUI
{
    public partial class App : Application
    {
        public App(BeFitDbContext context)
        {
            InitializeComponent();
            
            // Ensure database is created
            context.Database.EnsureCreated();

            MainPage = new AppShell();
        }
    }
}