using BlazorRestProject.Database;
using BlazorRestProject.Services;

namespace BlazorRestProject
{
    public partial class App : Application
    {
        private readonly ApodService _apodService;
        public App(ApodService apodService)
        {
            InitializeComponent();
            _apodService = apodService;

            SqliteDatabase.InitializeDatabaseAsync();
            Task.Run( async () => { await _apodService.FetchAndStoreApodImagesOfAllTimes(); });
            MainPage = new MainPage();
        }
    }
}
