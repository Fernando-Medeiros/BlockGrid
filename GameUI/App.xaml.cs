namespace GameUI;

public sealed partial class App : Application
{
    public App()
    {
        InitializeComponent();

        Dispatcher.DispatchAsync(ResourceContainer.LoadResourcesAsync);

        MainPage = new AppShell();
    }

    /// <summary>
    /// Gets the current <see cref="ResourceContainer"/> instance in use
    /// </summary>
    public static ResourceContainer ResourceContainer { get; } = new();
}
