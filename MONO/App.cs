using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace MONOGame;

public sealed class App : Game
{
    private SpriteBatch SpriteBatch;
    private readonly Camera<Vector2> Camera;
    private readonly GraphicsDeviceManager Graphics;

    public static Global Global { get; } = new();
    public static Resources Resources { get; } = new();

    private static IReadOnlyList<IList<INode2D>> Nodes { get; } =
        [[], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], []];

    public App()
    {
        IsMouseVisible = true;
        Content.RootDirectory = "content";

        var (width, height) = (1280, 960);

        Graphics = new(this)
        {
            PreferredBackBufferWidth = width,
            PreferredBackBufferHeight = height
        };
        Graphics.ApplyChanges();

        Camera = new OrthographicCamera(new BoxingViewportAdapter(Window, GraphicsDevice, width, height));
    }

    public void ConfigureListeners()
    {
        Window.KeyDown += (_, e) => Global.Invoke(CoreEvent.KeyPressed, Enum.GetName(e.Key));
        Window.KeyUp += (_, e) => Global.Invoke(CoreEvent.KeyReleased, Enum.GetName(e.Key));

        Global.Subscribe(CoreEvent.Camera, CameraChanged);
    }

    protected override void Initialize()
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
        base.Initialize();
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        Resources.SetContent(Content);
        Resources.LoadResources();
        Resources.LoadScene();
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());

        foreach (IList<INode2D> nodeRow in Nodes)
        {
            foreach (INode2D node in nodeRow)
            {
                node.Canva.Draw(SpriteBatch);
            }
        }
        SpriteBatch.End();

        base.Draw(gameTime);
    }

    private void CameraChanged(object? sender)
    {
        if (Is.Not<Position2D>(sender)) return;

        var (_, _, posX, posY) = (Position2D)sender!;

        float width = Camera.BoundingRectangle.Width;
        float height = Camera.BoundingRectangle.Height;

        float maxHeight = Global.MAX_ROW * Global.RECT;
        float maxWidth = Global.MAX_COLUMN * Global.RECT;

        float scrollX = posX - (width / 2);
        float scrollY = posY - (height / 2);

        scrollX = Math.Max(0, Math.Min(scrollX, maxWidth - width));
        scrollY = Math.Max(0, Math.Min(scrollY, maxHeight - height));

        Camera.LookAt(new Vector2(scrollX + (width / 2), scrollY + (height / 2)));
    }
}
