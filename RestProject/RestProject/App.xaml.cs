namespace RestProject;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

    }

    protected override Window CreateWindow(IActivationState activationState)
    {

        var appShell = new AppShell();

        var window = new Window(appShell)
        {
            Title = "NASA APOD Gallery"
        };

        return window;
    }
}