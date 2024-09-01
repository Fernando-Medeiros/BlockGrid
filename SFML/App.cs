namespace SFMLGame;

internal sealed class App
{
    private WorldView WorldView { get; }
    private LoggerBox LoggerBox { get; }
    private RenderWindow Window { get; }

    public App()
    {
        LoggerBox = new LoggerBox();
        WorldView = new WorldView(new FloatRect(0, 0, 1280, 960));

        Window = new RenderWindow(new VideoMode(1280, 960), "2D Game", Styles.Titlebar | Styles.Close);
        Window.SetFramerateLimit(30);
        Window.SetVerticalSyncEnabled(true);
    }

    public void ConfigureResources()
    {
        Content.LoadResources();
        WorldView.ConfigureNodes();
    }

    public void ConfigureListeners()
    {
        WorldView.ConfigureListeners(Window);
        LoggerBox.ConfigureListeners(Window);

        Window.Closed += (_, _) => Window.Close();
        Window.KeyPressed += (_, e) => Global.Invoke(EEvent.KeyPressed, Enum.GetName(e.Code));
        Window.KeyReleased += (_, e) => Global.Invoke(EEvent.KeyReleased, Enum.GetName(e.Code));
    }

    public void Start()
    {
        Content.LoadScene();

        while (Window.IsOpen)
        {
            Window.DispatchEvents();
            Window.SetView(WorldView);

            Window.Clear();

            WorldView.Draw(Window);
            LoggerBox.Draw(Window);

            Window.Display();
        }
    }
}