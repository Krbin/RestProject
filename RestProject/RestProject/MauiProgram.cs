using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using RestProject.Services;
using RestProject.ViewModels;
using RestProject.Data;
using System.Threading.Tasks;
using CommunityToolkit.Maui;

namespace RestProject
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
                })
                .UseMauiCommunityToolkit();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
            builder.Services.AddSingleton<INasaApiService, NasaApiService>();
            builder.Services.AddSingleton<ISynchronizationService, SynchronizationService>();
            builder.Services.AddSingleton<ISearchService, SearchService>();
            builder.Services.AddSingleton<ISharingService, SharingService>();
            builder.Services.AddSingleton<IImageSavingService, ImageSavingService>();

            builder.Services.AddScoped<SearchViewModel>();
            builder.Services.AddScoped<GroupedViewModel>();
            builder.Services.AddScoped<YearViewModel>();
            builder.Services.AddScoped<MonthViewModel>();
            builder.Services.AddScoped<ImageDetailViewModel>();

            var app = builder.Build();

            var syncService = app.Services.GetRequiredService<ISynchronizationService>();
            Task.Run(() => syncService.StartSyncIfNeededAsync());

            return app;
        }
    }
}