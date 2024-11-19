using RestProject.ViewModels;
using RestProject.Services;
using Microsoft.Maui.Controls;

namespace RestProject
{
    public partial class MainPage : ContentPage
    {
        private readonly ISqliteService _sqliteService;

        public MainPage(ISqliteService sqliteService)
        {
            InitializeComponent();
            _sqliteService = sqliteService;
            BindingContext = new ApodViewModel(sqliteService);
        }
    }
}