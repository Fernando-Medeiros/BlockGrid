namespace SFMLGame.core.widgets.shared;

public abstract class BaseButton<TGraphic>
    : IButton, IDisposable where TGraphic : Transformable, Drawable, IDisposable
{
    public event Action<object?>? OnChanged;
    public event Action<object?>? OnClicked;
    public event Action<object?>? OnFocused;

    #region SFML Property
    protected TGraphic Graphic { get; init; } // Init Required
    #endregion

    #region Control Property
    public bool Focused { get; set; }
    public bool Selected { get; set; }
    public bool Disposed { get; private set; }
    public bool Enabled { get; protected set; } = true;
    #endregion

    #region Required Property
    public object Id { get; init; } // Init Required
    public Vector2f Position { get; set; } // Required
    #endregion

    #region Custom Property
    public uint FontSize { get; set; } = 12;
    public float Outline { get; set; } = 1f;

    public EFont Font { get; set; } = EFont.Romulus;
    public EColor Color { get; set; } = EColor.White;
    public EColor OutlineColor { get; set; } = EColor.Black;
    public EColor FocusedColor { get; set; } = EColor.GoldRod;
    public EColor SelectedColor { get; set; } = EColor.CornFlowerBlue;
    public EColor BackgroundColor { get; set; } = EColor.Transparent;
    #endregion

    #region Action
    public bool Equal(object? value) => Id.Equals(value);

    public virtual void Activated(bool value) => Enabled = value;

    public float GetPosition(EDirection direction)
    {
        FloatRect rect = GetGlobalBounds();

        if (direction is EDirection.Top) return rect.Position.Y;
        if (direction is EDirection.Left) return rect.Position.X;
        if (direction is EDirection.Right) return rect.Position.X + rect.Width;
        if (direction is EDirection.Bottom) return rect.Position.Y + rect.Height;

        return rect.Position.Y + rect.Height;
    }
    #endregion

    #region Build
    public virtual void Event()
    {
        Global.Subscribe(EEvent.MouseMoved, OnFocusChanged);
        Global.Subscribe(EEvent.MouseButtonPressed, OnButtonClicked);
    }

    public virtual void Render(RenderWindow window)
    {
    }
    #endregion

    #region Event
    protected virtual void OnButtonClicked(object? sender)
    {
        if (Collidepoint(sender))
        {
            Selected = !Selected;
            OnClicked?.Invoke(Id);

            var sound = Content.GetResource<Sound>(ESound.ButtonClicked);
            sound.Volume = App.Configuration.SoundVolume;
            sound.Play();
        }
    }

    protected virtual void OnFocusChanged(object? sender)
    {
        if (Collidepoint(sender))
        {
            Focused = true;
            OnFocused?.Invoke(Id);
            return;
        }
        Focused = false;
    }

    protected virtual void OnTextEntered(object? sender)
    {
        OnChanged?.Invoke(Id);
    }
    #endregion

    #region AUX
    private bool Collidepoint(object? sender)
    {
        if (Disposed || Enabled is false || sender is null) return false;

        if (sender is MouseDTO mousePosition)
        {
            if (mousePosition.Button != EMouse.Left) return false;

            return GetGlobalBounds().Contains(mousePosition);
        }
        return false;
    }

    private FloatRect GetGlobalBounds(bool globalBounds = true)
    {
        return (globalBounds, Graphic) switch
        {
            (false, Text drawable) => drawable.GetLocalBounds(),
            (false, Shape drawable) => drawable.GetLocalBounds(),
            (false, Sprite drawable) => drawable.GetLocalBounds(),

            (true, Text drawable) => drawable.GetGlobalBounds(),
            (true, Shape drawable) => drawable.GetGlobalBounds(),
            (true, Sprite drawable) => drawable.GetGlobalBounds(),
            _ => new FloatRect()
        };
    }
    #endregion

    #region Dispose
    public virtual void Dispose()
    {
        Disposed = true;

        Graphic?.Dispose();
        OnClicked = null;
        OnFocused = null;
        OnChanged = null;
        Global.Unsubscribe(EEvent.MouseMoved, OnFocusChanged);
        Global.Unsubscribe(EEvent.MouseButtonPressed, OnButtonClicked);
    }
    #endregion
}
