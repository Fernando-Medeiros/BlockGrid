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
    public const string TITLE = "Realms of Storms";

    public const byte RECT = 32;
    public const byte MAX_ROW = 255;
    public const byte MAX_COLUMN = 255;
    public const int WINDOW_WIDTH = 1920;
    public const int WINDOW_HEIGHT = 1080;
    public const float WORLD_HEIGHT = MAX_ROW * RECT;
    public const float WORLD_WIDTH = MAX_COLUMN * RECT;

    #region Event
    private static event Action<object?>? OnScene;
    private static event Action<object?>? OnRegion;
    private static event Action<object?>? OnLogger;
    private static event Action<object?>? OnCamera;
    private static event Action<object?>? OnTransport;
    private static event Action<object?>? OnEndGame;
    private static event Action<object?>? OnSaveGame;
    private static event Action<object?>? OnBasicStatus;
    private static event Action<object?>? OnKeyPressed;
    private static event Action<object?>? OnKeyReleased;
    private static event Action<object?>? OnMouseMoved;
    private static event Action<object?>? OnMouseWheelScrolled;
    private static event Action<object?>? OnMouseButtonPressed;

    public static void Subscribe(EEvent e, Action<object?> action)
    {
        switch (e)
        {
            case EEvent.Scene: OnScene += action; break;
            case EEvent.Logger: OnLogger += action; break;
            case EEvent.Region: OnRegion += action; break;
            case EEvent.Camera: OnCamera += action; break;
            case EEvent.Transport: OnTransport += action; break;
            case EEvent.EndGame: OnEndGame += action; break;
            case EEvent.SaveGame: OnSaveGame += action; break;
            case EEvent.BasicStatus: OnBasicStatus += action; break;
            case EEvent.KeyPressed: OnKeyPressed += action; break;
            case EEvent.KeyReleased: OnKeyReleased += action; break;
            case EEvent.MouseMoved: OnMouseMoved += action; break;
            case EEvent.MouseWheelScrolled: OnMouseWheelScrolled += action; break;
            case EEvent.MouseButtonPressed: OnMouseButtonPressed += action; break;
        };
    }

    public static void UnSubscribe(EEvent e, Action<object?> action)
    {
        switch (e)
        {
            case EEvent.Scene: OnScene -= action; break;
            case EEvent.Logger: OnLogger -= action; break;
            case EEvent.Region: OnRegion -= action; break;
            case EEvent.Camera: OnCamera -= action; break;
            case EEvent.Transport: OnTransport -= action; break;
            case EEvent.EndGame: OnEndGame -= action; break;
            case EEvent.SaveGame: OnSaveGame -= action; break;
            case EEvent.BasicStatus: OnBasicStatus -= action; break;
            case EEvent.KeyPressed: OnKeyPressed -= action; break;
            case EEvent.KeyReleased: OnKeyReleased -= action; break;
            case EEvent.MouseMoved: OnMouseMoved -= action; break;
            case EEvent.MouseWheelScrolled: OnMouseWheelScrolled -= action; break;
            case EEvent.MouseButtonPressed: OnMouseButtonPressed -= action; break;
        };
    }

    public static void Invoke(EEvent e, object? sender)
    {
        switch (e)
        {
            case EEvent.Scene: OnScene?.Invoke(sender); break;
            case EEvent.Logger: OnLogger?.Invoke(sender); break;
            case EEvent.Region: OnRegion?.Invoke(sender); break;
            case EEvent.Camera: OnCamera?.Invoke(sender); break;
            case EEvent.Transport: OnTransport?.Invoke(sender); break;
            case EEvent.EndGame: OnEndGame?.Invoke(sender); break;
            case EEvent.SaveGame: OnSaveGame?.Invoke(sender); break;
            case EEvent.BasicStatus: OnBasicStatus?.Invoke(sender); break;
            case EEvent.KeyPressed: OnKeyPressed?.Invoke(sender); break;
            case EEvent.KeyReleased: OnKeyReleased?.Invoke(sender); break;
            case EEvent.MouseMoved: OnMouseMoved?.Invoke(sender); break;
            case EEvent.MouseWheelScrolled: OnMouseWheelScrolled?.Invoke(sender); break;
            case EEvent.MouseButtonPressed: OnMouseButtonPressed?.Invoke(sender); break;
        };
    }
    #endregion
}
