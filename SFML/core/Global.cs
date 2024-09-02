namespace SFMLGame.core;

public readonly struct Key
{
    public const string A = "A";
    public const string D = "D";
    public const string S = "S";
    public const string E = "E";
    public const string Q = "Q";
    public const string W = "W";
    public const string Up = "Up";
    public const string Left = "Left";
    public const string Right = "Right";
    public const string Down = "Down";
    public const string Space = "Space";

    public static readonly IReadOnlyList<string> Actions = [Space];
    public static readonly IReadOnlyList<string> Positions = [Up, Down, Left, Right];
}

public static class Global
{
    public const byte RECT = 32;
    public const byte MAX_ROW = 132;
    public const byte MAX_COLUMN = 132;

    #region Event
    private static event EventHandler<EventArgs?>? OnLogger;
    private static event EventHandler<EventArgs?>? OnCamera;
    private static event EventHandler<EventArgs?>? OnLoadScene;
    private static event EventHandler<EventArgs?>? OnKeyPressed;
    private static event EventHandler<EventArgs?>? OnKeyReleased;

    public static void Subscribe(EEvent e, Action<object?> routine)
    {
        if (e is EEvent.Logger) OnLogger += (args, _) => routine(args);
        if (e is EEvent.Camera) OnCamera += (args, _) => routine(args);
        if (e is EEvent.LoadScene) OnLoadScene += (args, _) => routine(args);
        if (e is EEvent.KeyPressed) OnKeyPressed += (args, _) => routine(args);
        if (e is EEvent.KeyReleased) OnKeyReleased += (args, _) => routine(args);
    }

    public static void UnSubscribe(EEvent e, Action<object?> routine)
    {
        if (e is EEvent.Logger) OnLogger -= (args, _) => routine(args);
        if (e is EEvent.Camera) OnCamera -= (args, _) => routine(args);
        if (e is EEvent.LoadScene) OnLoadScene -= (args, _) => routine(args);
        if (e is EEvent.KeyPressed) OnKeyPressed -= (args, _) => routine(args);
        if (e is EEvent.KeyReleased) OnKeyReleased -= (args, _) => routine(args);
    }

    public static void Invoke(EEvent e, object? args)
    {
        if (e is EEvent.Logger) OnLogger?.Invoke(args, default);
        if (e is EEvent.Camera) OnCamera?.Invoke(args, default);
        if (e is EEvent.LoadScene) OnLoadScene?.Invoke(args, default);
        if (e is EEvent.KeyPressed) OnKeyPressed?.Invoke(args, default);
        if (e is EEvent.KeyReleased) OnKeyReleased?.Invoke(args, default);
    }
    #endregion
}
