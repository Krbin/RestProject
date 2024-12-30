using System.Windows.Input;

namespace RestProject.Views;

public partial class MonthPage : ContentPage
{
    private readonly INavigation _navigation;
    private readonly ISqliteService _sqliteService;
    public ICommand FrameTappedCommand { get; }
    public MonthPage(string Month)
    {
        InitializeComponent();
        FrameTappedCommand = new Command<int>(OnFrameTapped);
    }
    private async void OnFrameTapped(int Id)
    {
        await _navigation.PushAsync(new ImagePage(Id));
    }
}