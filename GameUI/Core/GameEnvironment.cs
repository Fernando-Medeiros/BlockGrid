namespace GameUI.Core;

public readonly struct Key
{
    public const string A = "A";
    public const string D = "D";
    public const string S = "S";
    public const string W = "W";
    public const string Up = "Up";
    public const string Left = "Left";
    public const string Right = "Right";
    public const string Down = "Down";
    public const string Space = "Space";

    public static readonly IReadOnlyList<string> Actions = [Space];
    public static readonly IReadOnlyList<string> Positions = [Up, Down, Left, Right];
}

public sealed class GameEnvironment
{
    public const byte VECTOR = 38;
    public const byte MAX_ROW = 24;
    public const byte MAX_COLUMN = 44;

    #region Event
    private event EventHandler<EventArgs?>? OnKeyUp;
    private event EventHandler<EventArgs?>? OnKeyDown;
    private event EventHandler<EventArgs?>? OnLoadScene;
    private event EventHandler<EventArgs?>? OnLoadResource;

    public void Subscribe(Event e, Action<object?> routine)
    {
        if (e is Event.KeyUp) OnKeyUp += (args, _) => routine(args);
        if (e is Event.KeyDown) OnKeyDown += (args, _) => routine(args);
        if (e is Event.LoadScene) OnLoadScene += (args, _) => routine(args);
        if (e is Event.LoadResource) OnLoadResource += (args, _) => routine(args);
    }

    public void UnSubscribe(Event e, Action<object?> routine)
    {
        if (e is Event.KeyUp) OnKeyUp -= (args, _) => routine(args);
        if (e is Event.KeyDown) OnKeyDown -= (args, _) => routine(args);
        if (e is Event.LoadScene) OnLoadScene -= (args, _) => routine(args);
        if (e is Event.LoadResource) OnLoadResource -= (args, _) => routine(args);
    }

    public void Invoke(Event e, object? args)
    {
        if (e is Event.KeyUp) OnKeyUp?.Invoke(args, default);
        if (e is Event.KeyDown) OnKeyDown?.Invoke(args, default);
        if (e is Event.LoadScene) OnLoadScene?.Invoke(args, default);
        if (e is Event.LoadResource) OnLoadResource?.Invoke(args, default);
    }
    #endregion
}
