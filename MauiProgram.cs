using Microsoft.Extensions.Logging;
using StudentSystem.DataBase;
using StudentSystem.ViewModel;
using static StudentSystem.Term;

namespace StudentSystem
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

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();


            builder.Services.AddSingleton<TermRecord>();
            builder.Services.AddTransient<TermRecord>();

            builder.Services.AddSingleton<DatabaseHelper>();

            //builder.Services.AddTransient<Term>();

            return builder.Build();
        }
    }
}
