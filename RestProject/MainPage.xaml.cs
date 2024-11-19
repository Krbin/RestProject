using RestProject.Models;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using RestProject.ViewModels;
using System.Threading.Tasks;
using RestProject.Database;


namespace RestProject
{
    public partial class MainPage : ContentPage
    {
        private readonly ISqliteService _sqliteService;

        public MainPage(ISqliteService sqliteService)
        {
            InitializeComponent();
            _sqliteService = sqliteService;
        }

    }
}