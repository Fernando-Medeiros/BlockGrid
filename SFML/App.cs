namespace SFMLGame;

internal sealed class App
{
    private FloatRect Size { get; }
    private WorldView WorldView { get; }
    private WorldUIView WorldUIView { get; }
    private RenderWindow Window { get; }

    public App()
    {
        Size = new(0, 0, Global.WINDOW_WIDTH, Global.WINDOW_HEIGHT);

        WorldView = new(Size);
        WorldUIView = new(Size);

        Window = new(new(Global.WINDOW_WIDTH, Global.WINDOW_HEIGHT), Global.TITLE, Styles.Titlebar);
        Window.SetFramerateLimit(30);
        Window.SetVerticalSyncEnabled(true);
    }

    public void ConfigureResources()
    {
        Content.LoadResources();
        WorldView.ConfigureNodes();
        WorldUIView.Add(new PlayerBoxShape());
        WorldUIView.Add(new EnemyBoxShape());
        WorldUIView.Add(new LoggerBoxShape());
    }

    public void ConfigureListeners()
    {
        WorldView.ConfigureListeners(Window);
        WorldUIView.ConfigureListeners(Window);

        Window.Closed += (_, _) => Window.Close();
        Window.KeyPressed += (_, e) => { if (e.Code == Keyboard.Key.Escape) Window.Close(); };

        Window.KeyPressed += (_, e) => Global.Invoke(EEvent.KeyPressed, Enum.GetName(e.Code));
        Window.KeyReleased += (_, e) => Global.Invoke(EEvent.KeyReleased, Enum.GetName(e.Code));
    }

    public void Start()
    {
        Content.LoadScene();

        while (Window.IsOpen)
        {
            Window.DispatchEvents();
            Window.Clear();

            WorldView.Draw(Window);
            WorldUIView.Draw(Window);

            Window.Display();
        }
    }
}