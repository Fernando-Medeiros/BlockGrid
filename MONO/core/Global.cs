namespace MONOGame.Core;

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

public sealed class Global
{
    public const byte RECT = 32;
    public const byte MAX_ROW = 44;
    public const byte MAX_COLUMN = 44;

    #region Event
    private event EventHandler<EventArgs?>? OnCamera;
    private event EventHandler<EventArgs?>? OnKeyPressed;
    private event EventHandler<EventArgs?>? OnKeyReleased;
    private event EventHandler<EventArgs?>? OnLoadScene;
    private event EventHandler<EventArgs?>? OnLoadResource;

    public void Subscribe(CoreEvent e, Action<object?> routine)
    {
        if (e is CoreEvent.Camera) OnCamera += (args, _) => routine(args);
        if (e is CoreEvent.KeyPressed) OnKeyPressed += (args, _) => routine(args);
        if (e is CoreEvent.KeyReleased) OnKeyReleased += (args, _) => routine(args);
        if (e is CoreEvent.LoadScene) OnLoadScene += (args, _) => routine(args);
        if (e is CoreEvent.LoadResource) OnLoadResource += (args, _) => routine(args);
    }

    public void UnSubscribe(CoreEvent e, Action<object?> routine)
    {
        if (e is CoreEvent.Camera) OnCamera -= (args, _) => routine(args);
        if (e is CoreEvent.KeyPressed) OnKeyPressed -= (args, _) => routine(args);
        if (e is CoreEvent.KeyReleased) OnKeyReleased -= (args, _) => routine(args);
        if (e is CoreEvent.LoadScene) OnLoadScene -= (args, _) => routine(args);
        if (e is CoreEvent.LoadResource) OnLoadResource -= (args, _) => routine(args);
    }

    public void Invoke(CoreEvent e, object? args)
    {
        if (e is CoreEvent.Camera) OnCamera?.Invoke(args, default);
        if (e is CoreEvent.KeyPressed) OnKeyPressed?.Invoke(args, default);
        if (e is CoreEvent.KeyReleased) OnKeyReleased?.Invoke(args, default);
        if (e is CoreEvent.LoadScene) OnLoadScene?.Invoke(args, default);
        if (e is CoreEvent.LoadResource) OnLoadResource?.Invoke(args, default);
    }
    #endregion
}
