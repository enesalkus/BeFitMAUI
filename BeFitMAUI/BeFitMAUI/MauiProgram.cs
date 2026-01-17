using Microsoft.Extensions.Logging;

namespace BeFitMAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            
            builder.Services.AddDbContext<BeFitMAUI.Data.BeFitDbContext>();
            builder.Services.AddScoped<BeFitMAUI.Services.TrainingService>();
            builder.Services.AddScoped<BeFitMAUI.Services.ExerciseService>();
            builder.Services.AddScoped<BeFitMAUI.Services.StatsService>();

            builder.Services.AddTransient<BeFitMAUI.ViewModels.DashboardViewModel>();
            builder.Services.AddTransient<BeFitMAUI.Views.DashboardPage>();

            builder.Services.AddTransient<BeFitMAUI.ViewModels.SessionsViewModel>();
            builder.Services.AddTransient<BeFitMAUI.Views.SessionsPage>();

            builder.Services.AddTransient<BeFitMAUI.ViewModels.ExercisesViewModel>();
            builder.Services.AddTransient<BeFitMAUI.Views.ExercisesPage>();

            builder.Services.AddTransient<BeFitMAUI.ViewModels.AddSessionViewModel>();
            builder.Services.AddTransient<BeFitMAUI.Views.AddSessionPage>();

            builder.Services.AddTransient<BeFitMAUI.ViewModels.SessionDetailViewModel>();
            builder.Services.AddTransient<BeFitMAUI.Views.SessionDetailPage>();

            return builder.Build();
        }
    }
}
