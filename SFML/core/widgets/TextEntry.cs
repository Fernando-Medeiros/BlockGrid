namespace SFMLGame.core.widgets;

public sealed class TextEntry : IButton, IDisposable
{
    private Text? Graphic { get; set; }
    private RectangleShape? Border { get; set; }

    #region Control Property
    public bool LengthReached { get; private set; }
    public bool Activated { get; private set; } = true;
    public bool Focused { get; set; }
    public bool Selected { get; set; }
    #endregion

    #region Required Property
    public required object Id { get; init; }
    public string Text { get; set; } = string.Empty;
    public required string Placeholder { get; set; }
    public required Vector2f Position { get; set; }
    public required Vector2f BorderSize { get; set; }
    #endregion

    #region Custom Property
    public int Padding { get; } = 5;
    public uint Size { get; set; } = 12;
    public float Outline { get; set; } = 1f;

    public EFont Font { get; set; } = EFont.Romulus;
    public EColor Color { get; set; } = EColor.White;
    public EColor OutlineColor { get; set; } = EColor.Black;
    public EColor FocusedColor { get; set; } = EColor.GoldRod;
    public EColor SelectedColor { get; set; } = EColor.CornFlowerBlue;
    public EColor LengthReachedColor { get; set; } = EColor.Tomate;
    #endregion

    #region Action
    public bool Equal(object? value) => Id.Equals(value);
    public void SetActivated(bool value) => Activated = value;
    #endregion

    #region Build
    public void Event()
    {
        Global.Subscribe(EEvent.TextEntered, OnTextEntered);
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
            FillColor = Factory.Color(EColor.Transparent),
            OutlineColor = Focused ? Factory.Color(FocusedColor)
                : LengthReached ? Factory.Color(LengthReachedColor)
                : Factory.Color(Color),
        });

        window.Draw(Graphic = new()
        {
            CharacterSize = Size,
            OutlineThickness = Outline,
            Font = Content.GetResource<Font>(Font),
            Position = Position + new Vector2f(Padding, -Padding),
            OutlineColor = Factory.Color(OutlineColor),
            DisplayedString = Text.Length == 0 ? Placeholder : Text,
            FillColor = Focused ? Factory.Color(FocusedColor)
                : Selected ? Factory.Color(SelectedColor)
                : Factory.Color(Color),
        });
    }
    #endregion

    #region Event
    public event Action<object?>? OnChanged;
    public event Action<object?>? OnClicked;
    public event Action<object?>? OnFocused;

    private void OnTextEntered(object? sender)
    {
        if (Activated is false || Selected is false) return;

        LengthReached = Graphic?.GetLocalBounds().Width >= Border?.GetGlobalBounds().Width - (Padding * 4);

        string temp = $"{sender}";

        if (temp is Key.CEscape)
            Selected = false;

        else if (temp is Key.CBackspace)
        {
            Text = Text.Length > 0 ? Text.Remove(Text.Length - 1, 1) : Text;
            LengthReached = false;
        }

        else if (LengthReached)
            return;

        else if (temp is Key.CTab || temp is " ")
            Text += " ";

        else if (temp.Length > 0)
            Text += temp;

        OnChanged?.Invoke(Id);
    }

    private void OnButtonClicked(object? sender)
    {
        if (Activated is false) return;

        if (sender is MouseDTO mouse)
        {
            if (mouse.Button != EMouse.Left) return;

            if (Border?.GetGlobalBounds().Contains(mouse) ?? default)
            {
                Selected = true;
                OnClicked?.Invoke(Id);

                var sound = Content.GetResource<Sound>(ESound.ButtonClicked);
                sound.Volume = App.Configuration.SoundVolume;
                sound.Play();
                return;
            }

            Selected = false;
        }
    }

    private void OnFocusChanged(object? sender)
    {
        if (Activated is false) return;

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
        Global.Unsubscribe(EEvent.TextEntered, OnTextEntered);
        Global.Unsubscribe(EEvent.MouseMoved, OnFocusChanged);
        Global.Unsubscribe(EEvent.MouseButtonPressed, OnButtonClicked);
    }
    #endregion
}
