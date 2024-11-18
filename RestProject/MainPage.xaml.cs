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

        public MainPage()
        {
            InitializeComponent();
            SqliteDatabase.InitializeDatabaseAsync().Wait();
        }

    }
}