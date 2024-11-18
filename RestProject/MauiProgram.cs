﻿using RestProject.Database;
using Microsoft.Extensions.Logging;
using RestProject.Models;
using RestProject.Services;
using RestProject.ViewModels;

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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<ISqliteService, SqliteService>();
            builder.Services.AddSingleton<ApodService>();

            //builder.Services.AddSingleton<MainPage>();
            //builder.Services.AddSingleton<ApodViewModel>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            var build = builder.Build();

            //Task.Delay(1).Wait();
            return build;
        }
    }
}