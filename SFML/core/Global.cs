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
    public const string TITLE = "2D Game";
    public const byte MAX_COLUMN = 132;
    public const int WINDOW_HEIGHT = 960;
    public const int WINDOW_WIDTH = 1280;
    public const float WORLD_HEIGHT = MAX_ROW * RECT;
    public const float WORLD_WIDTH = MAX_COLUMN * RECT;

    #region Event
    private static event EventHandler<EventArgs?>? OnLogger;
    private static event EventHandler<EventArgs?>? OnCamera;
    private static event EventHandler<EventArgs?>? OnLoadScene;
    private static event EventHandler<EventArgs?>? OnKeyPressed;
    private static event EventHandler<EventArgs?>? OnKeyReleased;
    private static event EventHandler<EventArgs?>? OnBasicStatus;

    public static void Subscribe(EEvent e, Action<object?> action)
    {
        if (e is EEvent.Logger) OnLogger += (args, _) => action(args);
        if (e is EEvent.Camera) OnCamera += (args, _) => action(args);
        if (e is EEvent.LoadScene) OnLoadScene += (args, _) => action(args);
        if (e is EEvent.KeyPressed) OnKeyPressed += (args, _) => action(args);
        if (e is EEvent.KeyReleased) OnKeyReleased += (args, _) => action(args);
        if (e is EEvent.BasicStatus) OnBasicStatus += (args, _) => action(args);
    }

    public static void UnSubscribe(EEvent e, Action<object?> action)
    {
        if (e is EEvent.Logger) OnLogger -= (args, _) => action(args);
        if (e is EEvent.Camera) OnCamera -= (args, _) => action(args);
        if (e is EEvent.LoadScene) OnLoadScene -= (args, _) => action(args);
        if (e is EEvent.KeyPressed) OnKeyPressed -= (args, _) => action(args);
        if (e is EEvent.KeyReleased) OnKeyReleased -= (args, _) => action(args);
        if (e is EEvent.BasicStatus) OnBasicStatus -= (args, _) => action(args);
    }

    public static void Invoke(EEvent e, object? sender)
    {
        if (e is EEvent.Logger) OnLogger?.Invoke(sender, EventArgs.Empty);
        if (e is EEvent.Camera) OnCamera?.Invoke(sender, EventArgs.Empty);
        if (e is EEvent.LoadScene) OnLoadScene?.Invoke(sender, EventArgs.Empty);
        if (e is EEvent.KeyPressed) OnKeyPressed?.Invoke(sender, EventArgs.Empty);
        if (e is EEvent.KeyReleased) OnKeyReleased?.Invoke(sender, EventArgs.Empty);
        if (e is EEvent.BasicStatus) OnBasicStatus?.Invoke(sender, EventArgs.Empty);
    }
    #endregion
}
