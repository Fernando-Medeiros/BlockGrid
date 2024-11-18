using SFMLGame.core.scenes;

namespace SFMLGame.pipeline;

internal sealed partial class App
{
    private static RenderWindow Window { get; }
    private static Dictionary<EScene, IView> Scenes { get; }

    static App()
    {
        var size = new FloatRect(0, 0, Configuration.WindowWidth, Configuration.WindowHeight);

        Window = new(
            new VideoMode((uint)size.Width, (uint)size.Height),
            Global.TITLE,
            Styles.Fullscreen);

        Scenes = [];
        Scenes.Add(EScene.Main, new MainScene(size));
        Scenes.Add(EScene.World, new WorldScene(size));
    }

    public void ConfigureResources()
    {
        Scenes[CurrentScene].Build();
    }

    public void ConfigureListeners()
    {
        Scenes[CurrentScene].Event();
    }

    public void Start()
    {
        while (Window.IsOpen)
        {
            Window.SetFramerateLimit(Configuration.Frame);

            Window.DispatchEvents();

            Window.Clear(Factory.Color(EColor.Black));

            Scenes[CurrentScene].Render(Window);

            Window.Display();
        }
    }
}