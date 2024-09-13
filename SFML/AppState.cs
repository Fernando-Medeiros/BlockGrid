namespace SFMLGame;

internal static class AppState
{
    public static IBody2D? CurrentPlayer { get; set; }
    public static EScene CurrentScene { get; private set; } = EScene.Main;
    public static Position2D CurrentPosition { get; private set; } = Position2D.Empty();

    public static void LoadEvents(RenderWindow window)
    {
        OnCreate(window);
        OnStart(window);
        OnResume(window);
        OnPause(window);
        OnDestroy(window);
    }

    #region States
    private static void OnCreate(RenderWindow window)
    {
        window.KeyPressed += (_, e) =>
           Global.Invoke(EEvent.KeyPressed, Enum.GetName(e.Code));

        window.KeyReleased += (_, e) =>
            Global.Invoke(EEvent.KeyReleased, Enum.GetName(e.Code));

        window.MouseWheelScrolled += (_, e) =>
           Global.Invoke(EEvent.MouseWheelScrolled, new MouseDTO(Enum.Parse<EMouse>($"{e.Delta}"), e.X, e.Y));

        window.MouseButtonPressed += (_, e) =>
            Global.Invoke(EEvent.MouseButtonPressed, new MouseDTO(Enum.Parse<EMouse>(Enum.GetName(e.Button)), e.X, e.Y));
    }

    private static void OnStart(RenderWindow window) { }

    private static void OnResume(RenderWindow window)
    {
        Global.Subscribe(EEvent.Scene, (x) =>
            { if (x is EScene scene) CurrentScene = scene; });

        Global.Subscribe(EEvent.Camera, (x) =>
            { if (x is Position2D position) CurrentPosition = position; });
    }

    private static void OnPause(RenderWindow window) { }

    private static void OnDestroy(RenderWindow window)
    {
        Global.Subscribe(EEvent.EndGame, (x) => window.Close());
    }
    #endregion
}
