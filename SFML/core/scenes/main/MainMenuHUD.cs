namespace SFMLGame.core.scenes.main;

public sealed class MainMenuHUD : IHud, IDisposable
{
    #region Field
    private bool enable = true;
    #endregion

    #region Property
    private Text Title { get; set; } = new();
    private IList<IButton> Buttons { get; } = [];
    private Rect Rect { get; set; } = Rect.Empty;
    private RectangleShape Background { get; set; } = new();
    #endregion

    #region Build
    public void Build()
    {
        Rect = new(
            Width: 300f,
            Height: 500f,
            X: 100f,
            Y: 100f);

        foreach (var command in Enum.GetValues<EMainMenu>())
        {
            var gap = ((byte)command * 50f) + 20f;

            var text = command.ToString().Replace("_", " ");

            Buttons.Add(new TextButton()
            {
                Size = 25,
                Text = text,
                Id = command,
                OutlineThickness = 0f,
                OutlineColor = EColor.Transparent,
                Position = new(Rect.X + 10, Rect.Y + gap),
            });
        }

        Title = new Text()
        {
            CharacterSize = 30,
            DisplayedString = Global.TITLE,
            FillColor = Factory.Color(EColor.White),
            Font = core.Content.GetResource(EFont.Romulus),
            Position = new(Rect.X - 40, Rect.Y - (Rect.Height / 10f)),
        };

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
        window.Draw(Title);

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
        if (sender is EMainMenu.New_Game) OnClicked?.Invoke(EMainMenu.New_Game);
        if (sender is EMainMenu.Load_Game) OnClicked?.Invoke(EMainMenu.Load_Game);
        if (sender is EMainMenu.Options) OnClicked?.Invoke(EMainMenu.Options);
        if (sender is EMainMenu.Quit) Global.Invoke(EEvent.EndGame, null);
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

        Title.Dispose();
        Background.Dispose();
    }
    #endregion
}
