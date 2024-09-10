namespace SFMLGame;

internal static class AppState
{
    public static EScene CurrentScene { get; private set; }

    public static void LoadEvents()
    {
        Global.Subscribe(EEvent.Scene, (x) => { if (x is EScene scene) CurrentScene = scene; });
    }
}
