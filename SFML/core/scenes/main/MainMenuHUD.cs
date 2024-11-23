namespace SFMLGame.core.scenes.main;

public sealed class MainMenuHUD : IHud, IDisposable
{
    #region Field
    private bool enable = true;
    #endregion

    #region Property
    private IList<IButton> Buttons { get; } = [];
    private Rect Rect { get; set; } = Rect.Empty;
    private RectangleShape Background { get; set; } = new();
    #endregion

    #region Build
    public void Build()
    {
        Rect = new Rect()
            .WithSize(width: 500f, height: 700f, padding: 68f)
            .WithAlignment();

        float posY = Rect.HeightTop;

        TextButton titleButton = new(
            id: Global.TITLE,
            text: Global.TITLE,
            position: new(Rect.WidthLeft, posY))
        {
            Size = 40,
        };

        Buttons.Add(titleButton);

        posY = titleButton.HeightBottom();

        foreach (var command in Enum.GetValues<EMainMenu>())
        {
            TextButton textButton = new(
                id: command,
                text: $"{command}".Replace("_", " "),
                position: new(Rect.WidthLeft, posY))
            {
                Size = 30,
            };

            posY = textButton.HeightBottom();

            Buttons.Add(textButton);
        }

        Background = new RectangleShape()
        {
            Position = new(Rect.X, Rect.Y),
            Size = new(Rect.Width, Rect.Height),
            Texture = Content.GetResource<Sprite>(EGraphic.BackgroundHUD).Texture,
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

        foreach (IButton button in Buttons) button.SetActivated(enable);
    }
    #endregion

    #region Event
    public event Action<object?>? OnClicked;

    private void OnButtonClicked(object? sender)
    {
        if (enable is false) return;

        if (sender is EMainMenu.Options) OnClicked?.Invoke(EMainMenu.Options);
        if (sender is EMainMenu.New_Game) OnClicked?.Invoke(EMainMenu.New_Game);
        if (sender is EMainMenu.Load_Game) OnClicked?.Invoke(EMainMenu.Load_Game);
        if (sender is EMainMenu.Quit) Global.Invoke(EEvent.EndGameChanged, null);

        foreach (var button in Buttons.OfType<TextButton>())
            button.Selected = button.Id.Equals(sender);
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
