using System.Windows.Input;

namespace RestProject.Views;

public partial class YearPage : ContentPage
{
    private readonly INavigation _navigation;
    private readonly ISqliteService _sqliteService;
    public ICommand FrameTappedCommand { get; }
    public YearPage(string Year)
    {
        InitializeComponent();
        FrameTappedCommand = new Command<string>(OnFrameTapped);

    }
    private async void OnFrameTapped(string month)
    {
        await _navigation.PushAsync(new MonthPage(month));
    }
}