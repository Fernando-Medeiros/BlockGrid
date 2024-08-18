namespace SFMLGame;

internal sealed class App
{
    private View Camera { get; }
    private RenderWindow Window { get; }

    public static Global Global { get; } = new();
    public static Resources Resources { get; } = new();

    private static IReadOnlyList<IList<INode2D>> Nodes { get; } =
      [[], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], []];

    public App()
    {
        var mode = new VideoMode(1200, 720);

        Camera = new View(new FloatRect(0, 0, mode.Width, mode.Height));

        Window = new RenderWindow(mode, "2D Game", Styles.Titlebar | Styles.Close);

        Window.SetFramerateLimit(30);

        Window.KeyPressed += (_, e) => Global.Invoke(CoreEvent.KeyPressed, Enum.GetName(e.Code));
        Window.KeyReleased += (_, e) => Global.Invoke(CoreEvent.KeyReleased, Enum.GetName(e.Code));
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
        Global.Subscribe(CoreEvent.Camera, CameraChanged);
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

    private void CameraChanged(object? sender)
    {
        if (Is.Not<Position2D>(sender)) return;

        var (_, _, posX, posY) = (Position2D)sender!;

        var (width, height) = (Camera.Size.X, Camera.Size.Y);

        float maxHeight = Global.MAX_ROW * Global.RECT;
        float maxWidth = Global.MAX_COLUMN * Global.RECT;

        float scrollX = posX - (width / 2);
        float scrollY = posY - (height / 2);

        scrollX = Math.Max(0, Math.Min(scrollX, maxWidth - width));
        scrollY = Math.Max(0, Math.Min(scrollY, maxHeight - height));

        Camera.Center = new Vector2f(scrollX + (width / 2), scrollY + (height / 2));
    }
}