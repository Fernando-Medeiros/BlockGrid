namespace SFMLGame.core.widgets;

public sealed class ImageButton : IButton, IDisposable
{
    private Sprite? Graphic { get; set; }
    private bool Focused { get; set; } = false;
    private bool Enabled { get; set; } = true;

    #region Property
    public required object Id { get; init; }
    public required Enum Image { get; set; }
    public required Vector2f Position { get; set; }
    #endregion

    #region Action
    public void IsEnabled(bool value) => Enabled = value;
    #endregion

    #region Build
    public void LoadEvents()
    {
        Global.Subscribe(EEvent.MouseMoved, OnFocusChanged);
        Global.Subscribe(EEvent.MouseButtonPressed, OnButtonClicked);
    }

    public void Draw(RenderWindow window)
    {
        var sprite = Image switch
        {
            EIcon resource => Content.GetResource(resource),
            ESprite resource => Content.GetResource(resource),
            EPicture resource => Content.GetResource(resource),
            ETerrain resource => Content.GetResource(resource),
            EGraphic resource => Content.GetResource(resource),
            _ => throw new Exception()
        };

        sprite.Position = Position;
        sprite.Color = Factory.Color(EOpacity.Light);
        window.Draw(Graphic = sprite);

        if (Focused)
        {
            sprite = Content.GetResource(EGraphic.SelectedNode);
            sprite.Position = Position;
            sprite.Color = Factory.Color(EOpacity.Light);
            window.Draw(sprite);
        }
    }
    #endregion

    #region Event
    public event Action<object?>? OnClicked;
    public event Action<object?>? OnFocus;

    private void OnButtonClicked(object? sender)
    {
        if (Enabled is false) return;

        if (sender is MouseDTO mouse)
        {
            if (mouse.Button != EMouse.Left) return;

            if (Graphic?.GetGlobalBounds().Contains(mouse) ?? default)
            {
                OnClicked?.Invoke(Id);
                var sound = Content.GetResource(ESound.ButtonClicked);
                sound.Play();
            }
        }
    }

    private void OnFocusChanged(object? sender)
    {
        if (Enabled is false) return;

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
        Graphic = null;
        OnFocus = null;
        OnClicked = null;
        Global.UnSubscribe(EEvent.MouseMoved, OnFocusChanged);
        Global.UnSubscribe(EEvent.MouseButtonPressed, OnButtonClicked);
    }
    #endregion
}
