namespace SFMLGame.core.widgets;

public sealed class TextButton(object id) : IButton, IDisposable
{
    private Text? Graphic { get; set; }
    private bool Focused { get; set; } = false;

    #region Property
    public object Id { get; init; } = id;
    public uint Size { get; set; } = 12;
    public string Text { get; set; } = string.Empty;
    public EColor Color { get; set; } = EColor.White;
    public EFont Font { get; set; } = EFont.Romulus;
    public Vector2f Position { get; set; } = new Vector2f();
    public EColor FocusedColor { get; set; } = EColor.GoldRod;
    #endregion

    #region Build
    public void LoadEvents()
    {
        Global.Subscribe(EEvent.MouseMoved, OnFocusChanged);
        Global.Subscribe(EEvent.MouseButtonPressed, OnButtonClicked);
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(Graphic = new Text()
        {
            Position = Position,
            CharacterSize = Size,
            OutlineThickness = 1f,
            DisplayedString = Text,
            Font = Content.GetResource(Font),
            FillColor = Focused ? Factory.Color(FocusedColor) : Factory.Color(Color),
            OutlineColor = Factory.Color(EColor.Black),
        });
    }
    #endregion

    #region Event
    public event Action<object?>? OnClicked;
    public event Action<object?>? OnFocus;

    private void OnButtonClicked(object? sender)
    {
        if (sender is MouseDTO mouse)
        {
            if (mouse.Button != EMouse.Left) return;

            if (Graphic?.GetGlobalBounds().Contains(mouse) ?? default)
            {
                OnClicked?.Invoke(Id);
            }
        }
    }

    private void OnFocusChanged(object? sender)
    {
        if (sender is MouseDTO mouse)
        {
            if (Graphic?.GetGlobalBounds().Contains(mouse) ?? false)
            {
                Focused = true;
                OnFocus?.Invoke(Id);
                return;
            }

            Focused = false;
        }
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
        Graphic?.Dispose();
        Graphic = null;
        Global.UnSubscribe(EEvent.MouseMoved, OnFocusChanged);
        Global.UnSubscribe(EEvent.MouseButtonPressed, OnButtonClicked);
    }
    #endregion
}
