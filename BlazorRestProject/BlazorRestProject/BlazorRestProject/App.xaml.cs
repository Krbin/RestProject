using BlazorRestProject.Database;
using BlazorRestProject.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorRestProject
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }
        public static  ApodService _apodService;
        public static ISqliteService _sqliteService;
        public App(IServiceProvider serviceProvider, ApodService apodService, ISqliteService sqliteService)
        {
            InitializeComponent(); 
            Services = serviceProvider;
             _apodService = apodService;
            _sqliteService = sqliteService;

            Task.Run( async () => { await _apodService.FetchAndStoreApodImagesOfAllTimes(); });
            MainPage = new MainPage();
        }
    }
}
