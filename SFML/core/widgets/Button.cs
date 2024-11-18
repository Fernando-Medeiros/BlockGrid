namespace SFMLGame.core.widgets;

public sealed class Button : IButton, IDisposable
{
    private Text? Graphic { get; set; }
    private RectangleShape? Border { get; set; }

    #region Control Property
    public bool Activated { get; private set; } = true;
    public bool Focused { get; set; }
    public bool Disabled { get; set; }
    #endregion

    #region Required Property
    public required object Id { get; init; }
    public required string Text { get; set; }
    public required Vector2f Position { get; set; }
    public required Vector2f BorderSize { get; set; }
    public required EColor BackgroundColor { get; set; }
    public required EColor Color { get; set; }
    #endregion

    #region Custom Property
    public int Padding { get; } = 5;
    public uint Size { get; set; } = 12;
    public float Outline { get; set; } = 1f;

    public EFont Font { get; set; } = EFont.Romulus;
    public EColor DisabledColor { get; set; } = EColor.Gray;
    public EColor OutlineColor { get; set; } = EColor.Black;
    public EColor FocusedColor { get; set; } = EColor.GoldRod;
    #endregion

    #region Action
    public bool Equal(object? value) => Id.Equals(value);
    public void SetActivated(bool value) => Activated = value;
    #endregion

    #region Build
    public void Event()
    {
        Global.Subscribe(EEvent.MouseMoved, OnFocusChanged);
        Global.Subscribe(EEvent.MouseButtonPressed, OnButtonClicked);
    }

    public void Render(RenderWindow window)
    {
        window.Draw(Border = new()
        {
            Size = BorderSize,
            Position = Position,
            OutlineThickness = Outline,
            OutlineColor = Factory.Color(OutlineColor),
            FillColor = Disabled ? Factory.Color(DisabledColor)
                : Focused ? Factory.Color(FocusedColor)
                : Factory.Color(BackgroundColor),
        });

        window.Draw(Graphic = new()
        {
            CharacterSize = Size,
            DisplayedString = Text,
            OutlineThickness = Outline,
            Font = Content.GetResource<Font>(Font),
            OutlineColor = Factory.Color(OutlineColor),
            Position = Position + new Vector2f(BorderSize.X / 3f, -Padding),
            FillColor = Disabled ? Factory.Color(DisabledColor) : Factory.Color(Color),
        });
    }
    #endregion

    #region Event
    public event Action<object?>? OnChanged;
    public event Action<object?>? OnClicked;
    public event Action<object?>? OnFocused;

    private void OnButtonClicked(object? sender)
    {
        if (Activated is false || Disabled) return;

        if (sender is MouseDTO mouse)
        {
            if (mouse.Button != EMouse.Left) return;

            if (Border?.GetGlobalBounds().Contains(mouse) ?? default)
            {
                OnClicked?.Invoke(Id);

                var sound = Content.GetResource<Sound>(ESound.ButtonClicked);
                sound.Volume = App.Configuration.SoundVolume;
                sound.Play();
            }
        }
    }

    private void OnFocusChanged(object? sender)
    {
        if (Activated is false || Disabled) return;

        if (sender is MouseDTO mouse)
        {
            if (Border?.GetGlobalBounds().Contains(mouse) ?? false)
            {
                Focused = true;
                OnFocused?.Invoke(Id);
                return;
            }

            Focused = false;
        }
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
        Border?.Dispose();
        Border = null;
        Graphic?.Dispose();
        Graphic = null;
        OnClicked = null;
        OnFocused = null;
        OnChanged = null;
        Global.Unsubscribe(EEvent.MouseMoved, OnFocusChanged);
        Global.Unsubscribe(EEvent.MouseButtonPressed, OnButtonClicked);
    }
    #endregion
}
