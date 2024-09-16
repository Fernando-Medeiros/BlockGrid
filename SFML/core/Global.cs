namespace SFMLGame.core;

public readonly struct Key
{
    public const string A = "A";
    public const string D = "D";
    public const string S = "S";
    public const string X = "X";
    public const string Z = "Z";
    public const string E = "E";
    public const string Q = "Q";
    public const string W = "W";
    public const string Up = "Up";
    public const string Left = "Left";
    public const string Right = "Right";
    public const string Down = "Down";
    public const string Space = "Space";
    public const string Escape = "Escape";

    public static readonly IReadOnlyList<string> Actions = [Space];
    public static readonly IReadOnlyList<string> Movement = [W, S, A, D];
}

public static class Global
{
    public const byte RECT = 32;
    public const byte MAX_ROW = 255;
    public const string TITLE = "2D Game";
    public const byte MAX_COLUMN = 255;
    public const int WINDOW_HEIGHT = 1080;
    public const int WINDOW_WIDTH = 1920;
    public const float WORLD_HEIGHT = MAX_ROW * RECT;
    public const float WORLD_WIDTH = MAX_COLUMN * RECT;

    #region Event

    private static event EventHandler<EventArgs?>? OnScene;
    private static event EventHandler<EventArgs?>? OnRegion;
    private static event EventHandler<EventArgs?>? OnLogger;
    private static event EventHandler<EventArgs?>? OnCamera;
    private static event EventHandler<EventArgs?>? OnTransport;
    private static event EventHandler<EventArgs?>? OnEndGame;
    private static event EventHandler<EventArgs?>? OnSaveGame;
    private static event EventHandler<EventArgs?>? OnBasicStatus;
    private static event EventHandler<EventArgs?>? OnKeyPressed;
    private static event EventHandler<EventArgs?>? OnKeyReleased;
    private static event EventHandler<EventArgs?>? OnMouseWheelScrolled;
    private static event EventHandler<EventArgs?>? OnMouseButtonPressed;

    public static void Subscribe(EEvent e, Action<object?> action)
    {
        if (e is EEvent.Scene) OnScene += (args, _) => action(args);
        if (e is EEvent.Logger) OnLogger += (args, _) => action(args);
        if (e is EEvent.Region) OnRegion += (args, _) => action(args);
        if (e is EEvent.Camera) OnCamera += (args, _) => action(args);
        if (e is EEvent.Transport) OnTransport += (args, _) => action(args);
        if (e is EEvent.EndGame) OnEndGame += (args, _) => action(args);
        if (e is EEvent.SaveGame) OnSaveGame += (args, _) => action(args);
        if (e is EEvent.BasicStatus) OnBasicStatus += (args, _) => action(args);
        if (e is EEvent.KeyPressed) OnKeyPressed += (args, _) => action(args);
        if (e is EEvent.KeyReleased) OnKeyReleased += (args, _) => action(args);
        if (e is EEvent.MouseWheelScrolled) OnMouseWheelScrolled += (args, _) => action(args);
        if (e is EEvent.MouseButtonPressed) OnMouseButtonPressed += (args, _) => action(args);
    }

    public static void UnSubscribe(EEvent e, Action<object?> action)
    {
        if (e is EEvent.Scene) OnScene -= (args, _) => action(args);
        if (e is EEvent.Logger) OnLogger -= (args, _) => action(args);
        if (e is EEvent.Region) OnRegion -= (args, _) => action(args);
        if (e is EEvent.Camera) OnCamera -= (args, _) => action(args);
        if (e is EEvent.Transport) OnTransport -= (args, _) => action(args);
        if (e is EEvent.EndGame) OnEndGame -= (args, _) => action(args);
        if (e is EEvent.SaveGame) OnSaveGame -= (args, _) => action(args);
        if (e is EEvent.BasicStatus) OnBasicStatus -= (args, _) => action(args);
        if (e is EEvent.KeyPressed) OnKeyPressed -= (args, _) => action(args);
        if (e is EEvent.KeyReleased) OnKeyReleased -= (args, _) => action(args);
        if (e is EEvent.MouseWheelScrolled) OnMouseWheelScrolled -= (args, _) => action(args);
        if (e is EEvent.MouseButtonPressed) OnMouseButtonPressed -= (args, _) => action(args);
    }

    public static void Invoke(EEvent e, object? sender)
    {
        if (e is EEvent.Scene) OnScene?.Invoke(sender, EventArgs.Empty);
        if (e is EEvent.Logger) OnLogger?.Invoke(sender, EventArgs.Empty);
        if (e is EEvent.Region) OnRegion?.Invoke(sender, EventArgs.Empty);
        if (e is EEvent.Camera) OnCamera?.Invoke(sender, EventArgs.Empty);
        if (e is EEvent.Transport) OnTransport?.Invoke(sender, EventArgs.Empty);
        if (e is EEvent.EndGame) OnEndGame?.Invoke(sender, EventArgs.Empty);
        if (e is EEvent.SaveGame) OnSaveGame?.Invoke(sender, EventArgs.Empty);
        if (e is EEvent.BasicStatus) OnBasicStatus?.Invoke(sender, EventArgs.Empty);
        if (e is EEvent.KeyPressed) OnKeyPressed?.Invoke(sender, EventArgs.Empty);
        if (e is EEvent.KeyReleased) OnKeyReleased?.Invoke(sender, EventArgs.Empty);
        if (e is EEvent.MouseWheelScrolled) OnMouseWheelScrolled?.Invoke(sender, EventArgs.Empty);
        if (e is EEvent.MouseButtonPressed) OnMouseButtonPressed?.Invoke(sender, EventArgs.Empty);
    }
    #endregion
}
