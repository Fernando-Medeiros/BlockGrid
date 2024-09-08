using SFMLGame.core.scenes.main;
using SFMLGame.core.scenes.world;

namespace SFMLGame;

internal sealed class App
{
    private FloatRect Size { get; }
    private RenderWindow Window { get; }
    private EScene CurrentScene { get; set; }
    private Dictionary<EScene, IList<IGameObject>> Scenes { get; }

    public App()
    {
        Size = new(0, 0, Global.WINDOW_WIDTH, Global.WINDOW_HEIGHT);

        Window = new(new(Global.WINDOW_WIDTH, Global.WINDOW_HEIGHT), Global.TITLE, Styles.Fullscreen);

        Window.SetFramerateLimit(30);
        Window.SetVerticalSyncEnabled(false);

        Scenes = [];
        Scenes.Add(EScene.Main, [new MainUIView(Size)]);
        Scenes.Add(EScene.World, [new WorldView(Size), new WorldUIView(Size)]);
    }

    public void ConfigureResources()
    {
        Content.LoadResources();

        foreach (var sceneObjects in Scenes.Values)
            foreach (var gameObject in sceneObjects)
                gameObject.LoadContent();
    }

    public void ConfigureListeners()
    {
        foreach (var sceneObjects in Scenes.Values)
            foreach (var gameObject in sceneObjects)
                gameObject.LoadEvents();

        Window.KeyPressed += (_, e) =>
            Global.Invoke(EEvent.KeyPressed, Enum.GetName(e.Code));

        Window.KeyReleased += (_, e) =>
            Global.Invoke(EEvent.KeyReleased, Enum.GetName(e.Code));

        Window.MouseWheelScrolled += (_, e) =>
           Global.Invoke(EEvent.MouseWheelScrolled, new MouseDTO(Enum.Parse<EMouse>($"{e.Delta}"), e.X, e.Y));

        Window.MouseButtonPressed += (_, e) =>
            Global.Invoke(EEvent.MouseButtonPressed, new MouseDTO(Enum.Parse<EMouse>(Enum.GetName(e.Button)), e.X, e.Y));

        Global.Subscribe(EEvent.EndGame, (x) => Window.Close());

        Global.Subscribe(EEvent.Scene, (x) => { if (x is EScene scene) CurrentScene = scene; });
    }

    public void Start()
    {
        while (Window.IsOpen)
        {
            Window.DispatchEvents();
            Window.Clear();

            foreach (var gameObject in Scenes[CurrentScene])
                gameObject.Draw(Window);

            Window.Display();
        }
    }
}