using RestProject.Views; // Add this using directive

namespace RestProject;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Define routes for pages reachable via navigation but not directly in the Shell structure (TabBar/Flyout)
        // Using nameof() provides better compile-time checking if view names change
        Routing.RegisterRoute(nameof(YearView), typeof(YearView));
        Routing.RegisterRoute(nameof(MonthView), typeof(MonthView));
        Routing.RegisterRoute(nameof(ImageDetailView), typeof(ImageDetailView));

        // Alternative string-based registration (matches NavigationManager paths used in ViewModels)
        // Make sure these match the strings used in NavigationManager.NavigateTo calls
        Routing.RegisterRoute("year", typeof(YearView));
        Routing.RegisterRoute("month", typeof(MonthView));
        Routing.RegisterRoute("image", typeof(ImageDetailView));
    }
}