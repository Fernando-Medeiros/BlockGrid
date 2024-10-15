namespace SFMLGame.core.scenes.main;

public sealed class NewGameHUD : IHud
{
    #region Field
    private bool enable;
    #endregion

    #region Property
    private IList<IButton> Buttons { get; } = [];
    private Rect Rect { get; set; } = Rect.Empty;
    private RectangleShape Background { get; set; } = new();
    #endregion

    #region Build
    public void Build()
    {
        Rect = new(
            Width: 500f,
            Height: 600f,
            X: App.CurrentWidth / 2f,
            Y: App.CurrentHeight / 3f);

        Buttons.Add(new ImageButton()
        {
            Id = EIcon.Close,
            Image = EIcon.Close,
            Position = new(Rect.X + Global.RECT + (Rect.Width / 2f), Rect.Y - Global.RECT),
        });

        Background = new RectangleShape()
        {
            Size = new(Rect.Width, Rect.Height),
            Position = new(Rect.X - (Rect.Width / 3.5f), Rect.Y - (Rect.Height / 6f)),
            Texture = core.Content.GetResource(EGraphic.BackgroundHUD).Texture,
        };
    }

    public void Event()
    {
        foreach (IButton button in Buttons)
        {
            button.Event();
            button.OnClicked += OnButtonClicked;
        }
    }

    public void Render(RenderWindow window)
    {
        if (enable is false) return;

        window.Draw(Background);

        foreach (IButton button in Buttons) button.Render(window);
    }
    #endregion

    #region State
    public void VisibilityChanged()
    {
        enable = !enable;

        foreach (IButton button in Buttons) button.Enabled(enable);
    }
    #endregion

    #region Event
    public event Action<object?>? OnClicked;

    private void OnButtonClicked(object? sender)
    {
        if (sender is EIcon.Close)
            OnClicked?.Invoke(EMainMenu.New_Game);
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
        OnClicked = null;

        foreach (IButton button in Buttons)
        {
            button.OnClicked -= OnButtonClicked;
            button.Dispose();
        }

        Buttons.Clear();
        Background.Dispose();
    }
    #endregion
}
