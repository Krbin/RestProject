using RestProject.Views;
using System.Windows.Input;

namespace RestProject
{
    public partial class MainPage : ContentPage
    {
        private readonly ISqliteService _sqliteService;
        public ICommand FrameTappedCommand { get; }
        private readonly INavigation _navigation;

        public MainPage(ISqliteService sqliteService)
        {
            InitializeComponent();
            FrameTappedCommand = new Command<string>(OnFrameTapped);
            _sqliteService = sqliteService;
            BindingContext = new ApodViewModel(sqliteService);
        }
        private async void OnFrameTapped(string year)
        {
            await _navigation.PushAsync(new YearPage(year));
        }
    }
}