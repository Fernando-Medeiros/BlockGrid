using SFMLGame.core.scenes.main;
using SFMLGame.core.scenes.world;

namespace SFMLGame;

internal sealed partial class App
{
    private static RenderWindow Window { get; }
    private static Dictionary<EScene, IGameObject> Scenes { get; }

    static App()
    {
        var size = new FloatRect(0, 0, Global.WINDOW_WIDTH, Global.WINDOW_HEIGHT);

        Window = new(new(Global.WINDOW_WIDTH, Global.WINDOW_HEIGHT), Global.TITLE, Styles.Fullscreen);
        Window.SetFramerateLimit(30);

        Scenes = [];
        Scenes.Add(EScene.Main, new MainScene(size));
        Scenes.Add(EScene.World, new WorldScene(size));
    }

    public void ConfigureResources()
    {
        Scenes[EScene.Main].LoadContent();
    }

    public void ConfigureListeners()
    {
        OnCreate();
        OnStart();
        OnResume();
        OnPause();
        OnDestroy();

        Scenes[EScene.Main].LoadEvents();
    }

    public void Start()
    {
        while (Window.IsOpen)
        {
            Window.DispatchEvents();
            Window.Clear(Factory.Color(EColor.Black));

            Scenes[CurrentScene].Draw(Window);

            Window.Display();
        }
    }
}