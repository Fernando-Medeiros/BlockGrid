namespace SFMLGame;

internal sealed class App
{
    private FloatRect Size { get; }
    private IGameObject WorldView { get; }
    private IGameObject WorldUIView { get; }
    private RenderWindow Window { get; }

    public App()
    {
        Size = new(0, 0, Global.WINDOW_WIDTH, Global.WINDOW_HEIGHT);

        WorldView = new WorldView(Size);
        WorldUIView = new WorldUIView(Size);

        Window = new(new(Global.WINDOW_WIDTH, Global.WINDOW_HEIGHT), Global.TITLE, Styles.Titlebar);
        Window.SetFramerateLimit(30);
        Window.SetVerticalSyncEnabled(true);
    }

    public void ConfigureResources()
    {
        Content.LoadResources();
        WorldView.LoadContent();
        WorldUIView.LoadContent();
    }

    public void ConfigureListeners()
    {
        WorldView.LoadEvents(Window);
        WorldUIView.LoadEvents(Window);

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