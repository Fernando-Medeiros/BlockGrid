using SFMLGame.core;

namespace SFMLGame;

internal sealed class App
{
    private Camera2D Camera { get; }
    private RenderWindow Window { get; }

    public static Global Global { get; } = new();
    public static Resources Resources { get; } = new();

    private static IReadOnlyList<IList<INode2D>> Nodes { get; } =
      [
        [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [],
        [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [],
        [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [],
        [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [],
        [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [],
        [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [],
    ];

    public App()
    {
        var mode = new VideoMode(1280, 960);

        Camera = new(new FloatRect(0, 0, mode.Width, mode.Height));

        Window = new RenderWindow(mode, "2D Game", Styles.Titlebar | Styles.Close);

        Window.SetFramerateLimit(30);
    }

    public void ConfigureResources()
    {
        Resources.LoadResources();
    }

    public void ConfigureNodes()
    {
        for (byte row = 0; row < Global.MAX_ROW; row++)
        {
            for (byte column = 0; column < Global.MAX_COLUMN; column++)
            {
                var position = new Position2D(row, column, column * Global.RECT, row * Global.RECT);

                Nodes[row].Add(new Node2D(position));
            }
        }

        foreach (IList<INode2D> nodeRow in Nodes)
        {
            foreach (INode2D node in nodeRow)
            {
                node.Mount(Nodes);
            }
        }
    }

    public void ConfigureListeners()
    {
        Window.Closed += (_, _) => Window.Close();
        Window.KeyPressed += (_, e) => Global.Invoke(CoreEvent.KeyPressed, Enum.GetName(e.Code));
        Window.KeyReleased += (_, e) => Global.Invoke(CoreEvent.KeyReleased, Enum.GetName(e.Code));
        Window.MouseWheelScrolled += Camera.OnZoomChanged;

        Global.Subscribe(CoreEvent.Camera, Camera.OnCenterChanged);
    }

    public void Start()
    {
        Resources.LoadScene();

        while (Window.IsOpen)
        {
            Window.DispatchEvents();
            Window.SetView(Camera);
            Window.Clear();

            foreach (IList<INode2D> nodeRow in Nodes)
            {
                foreach (INode2D node in nodeRow)
                {
                    node.Canva.Draw(Window);
                }
            }

            Window.Display();
        }
    }
}