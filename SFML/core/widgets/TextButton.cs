namespace SFMLGame.core.widgets;

public sealed class TextButton(object id) : IButton, IDisposable
{
    private Text? Graphic { get; set; }
    private bool Selected { get; set; } = false;

    #region Property
    public object Id { get; init; } = id;
    public uint Size { get; set; } = 12;
    public string Text { get; set; } = string.Empty;
    public EColor Color { get; set; } = EColor.White;
    public EFont Font { get; set; } = EFont.Romulus;
    public Vector2f Position { get; set; } = new Vector2f();
    public EColor SelectedColor { get; set; } = EColor.GoldRod;
    #endregion

    #region Build
    public void LoadEvents()
    {
        Global.Subscribe(EEvent.MouseMoved, OnButtonSelected);
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
            FillColor = Selected ? Factory.Color(SelectedColor) : Factory.Color(Color),
            OutlineColor = Factory.Color(EColor.Black),
        });
    }
    #endregion

    #region Event
    public event EventHandler<EventArgs>? OnClicked;
    public event EventHandler<EventArgs>? OnSelected;

    private void OnButtonClicked(object? sender)
    {
        if (sender is MouseDTO mouse)
        {
            if (mouse.Button != EMouse.Left) return;

            if (Graphic?.GetGlobalBounds().Contains(mouse) ?? default)
            {
                OnClicked?.Invoke(Id, EventArgs.Empty);
            }
        }
    }

    private void OnButtonSelected(object? sender)
    {
        if (sender is MouseDTO mouse)
        {
            Selected = Graphic?.GetGlobalBounds().Contains(mouse) ?? false;

            if (Selected) OnSelected?.Invoke(true, EventArgs.Empty);
        }
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
        Graphic?.Dispose();
        Graphic = null;
        Global.UnSubscribe(EEvent.MouseMoved, OnButtonSelected);
        Global.UnSubscribe(EEvent.MouseButtonPressed, OnButtonClicked);
    }
    #endregion
}
