using BlazorRestProject.Database;
using BlazorRestProject.Services;
using BlazorRestProject.ViewModels;
using Microsoft.Extensions.Logging;

namespace BlazorRestProject
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
                });

            builder.Services.AddMauiBlazorWebView();


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<ISqliteService, SqliteService>();
            builder.Services.AddSingleton<ApodService>();

            builder.Services.AddSingleton<ApodViewModel>();

            SQLitePCL.Batteries_V2.Init();

            var app = builder.Build();
            InitializeDatabse();

            return app;
        }
        
        private static async void InitializeDatabse()
        {
            await SqliteDatabase.InitializeDatabaseAsync();
        }
    }
}
