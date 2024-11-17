namespace SFMLGame.core.widgets;

public sealed class ImageButton : IButton, IDisposable
{
    private Sprite? Graphic { get; set; }

    #region Control Property
    public bool Activated { get; private set; } = true;
    public bool Focused { get; set; }
    public bool Selected { get; set; }
    #endregion

    #region Required Property
    public required object Id { get; init; }
    public required Enum Image { get; set; }
    public required Vector2f Position { get; set; }
    #endregion

    #region Custom Property
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
        var sprite = Image switch
        {
            EIcon resource => Content.GetResource(resource),
            ERace resource => Content.GetResource(resource),
            ESprite resource => Content.GetResource(resource),
            EPicture resource => Content.GetResource(resource),
            ETerrain resource => Content.GetResource(resource),
            EGraphic resource => Content.GetResource(resource),
            EAlignment resource => Content.GetResource(resource),
            EProfession resource => Content.GetResource(resource),
            EProficiency resource => Content.GetResource(resource),
            _ => throw new Exception()
        };

        sprite.Position = Position;
        sprite.Color = Factory.Color(EOpacity.Light);
        window.Draw(Graphic = sprite);

        if (Focused || Selected)
        {
            sprite = Content.GetResource(EGraphic.SelectedNode);
            sprite.Position = Position;
            sprite.Color = Factory.Color(EOpacity.Light);
            window.Draw(sprite);
        }
    }
    #endregion

    #region Event
    public event Action<object?>? OnChanged;
    public event Action<object?>? OnClicked;
    public event Action<object?>? OnFocused;

    private void OnButtonClicked(object? sender)
    {
        if (Activated is false) return;

        if (sender is MouseDTO mouse)
        {
            if (mouse.Button != EMouse.Left) return;

            if (Graphic?.GetGlobalBounds().Contains(mouse) ?? default)
            {
                Selected = !Selected;
                OnClicked?.Invoke(Id);

                var sound = Content.GetResource(ESound.ButtonClicked);
                sound.Volume = App.CurrentSoundVolume;
                sound.Play();
            }
        }
    }

    private void OnFocusChanged(object? sender)
    {
        if (Activated is false) return;

        if (sender is MouseDTO mouse)
        {
            if (Graphic?.GetGlobalBounds().Contains(mouse) ?? false)
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
        Graphic = null;
        OnClicked = null;
        OnFocused = null;
        OnChanged = null;
        Global.UnSubscribe(EEvent.MouseMoved, OnFocusChanged);
        Global.UnSubscribe(EEvent.MouseButtonPressed, OnButtonClicked);
    }
    #endregion
}
