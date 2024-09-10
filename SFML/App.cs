using SFMLGame.core.scenes.main;
using SFMLGame.core.scenes.world;

namespace SFMLGame;

internal sealed class App
{
    private RenderWindow Window { get; }
    private Dictionary<EScene, IGameObject> Scenes { get; }

    public App()
    {
        var size = new FloatRect(0, 0, Global.WINDOW_WIDTH, Global.WINDOW_HEIGHT);

        Window = new(new((uint)size.Width, (uint)size.Height), Global.TITLE, Styles.Fullscreen);
        Window.SetFramerateLimit(30);

        Scenes = [];
        Scenes.Add(EScene.Main, new MainScene(size));
        Scenes.Add(EScene.World, new WorldScene(size));
    }

    public void ConfigureResources()
    {
        Content.LoadResources();

        foreach (var scene in Scenes.Values)
            scene.LoadContent();
    }

    public void ConfigureListeners()
    {
        AppState.LoadEvents(Window);

        foreach (var scene in Scenes.Values)
            scene.LoadEvents();
    }

    public void Start()
    {
        while (Window.IsOpen)
        {
            Window.DispatchEvents();
            Window.Clear();

            Scenes[AppState.CurrentScene].Draw(Window);

            Window.Display();
        }
    }
}