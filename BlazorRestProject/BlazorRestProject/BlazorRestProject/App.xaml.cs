using BlazorRestProject.Database;
using BlazorRestProject.Models;
using BlazorRestProject.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;

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
            
            var apodModel = new ApodModel();
            apodModel.ExplanationEnglish = "This is a test";
            apodModel.TitleEnglish = "test";
            apodModel.Url = "https://apod.nasa.gov/apod/image/2501/Mimas_Cassini_1800.jpg";
            apodModel.Year = "2025";
            
            Services = serviceProvider;
             _apodService = apodService;
            _sqliteService = sqliteService;

            Task.Run( async () => { await _apodService.FetchAndStoreApodImagesOfAllTimes(); });
            Task.Run(async () => { await _sqliteService.InsertDataAsync<ApodModel>(apodModel); });
            MainPage = new MainPage();
        }
    }
}
