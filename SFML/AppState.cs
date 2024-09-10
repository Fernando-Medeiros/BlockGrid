namespace SFMLGame;

internal static class AppState
{
    public static EScene CurrentScene { get; private set; }
    public static IBody2D? CurrentPlayer { get; private set; }

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
        Global.Subscribe(EEvent.Scene, (x) => { if (x is EScene scene) CurrentScene = scene; });
    }

    private static void OnPause(RenderWindow window) { }

    private static void OnDestroy(RenderWindow window)
    {
        Global.Subscribe(EEvent.EndGame, (x) => window.Close());
    }
    #endregion
}
