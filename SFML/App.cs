using SFMLGame.core.scenes.main;
using SFMLGame.core.scenes.world;

namespace SFMLGame;

internal sealed partial class App
{
    private static RenderWindow Window { get; }
    private static Dictionary<EScene, IView> Scenes { get; }

    static App()
    {
        var size = new FloatRect(0, 0, CurrentWidth, CurrentHeight);

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
        Scenes[EScene.Main].Build();
    }

    public void ConfigureListeners()
    {
        LoadEvents();

        Scenes[EScene.Main].Event();
    }

    public void Start()
    {
        while (Window.IsOpen)
        {
            if (CurrentMusic != null)
                CurrentMusic.Volume = CurrentMusicVolume;

            Window.SetFramerateLimit(CurrentFrame);

            Window.DispatchEvents();
            Window.Clear(Factory.Color(EColor.Black));

            Scenes[CurrentScene].Render(Window);

            Window.Display();
        }
    }
}