namespace SFMLGame.core.scenes.main;

public sealed class CommandHUD : IGameObject, IDisposable
{
    private enum ECmd : byte { NewGame, LoadGame, Options, Quit }

    #region Property
    private Text Title { get; set; } = new();
    private bool Enabled { get; set; } = true;
    private IList<IButton> Buttons { get; } = [];
    private Rect Rect { get; set; } = Rect.Empty;
    private RectangleShape Background { get; set; } = new();
    #endregion

    #region Build
    public void LoadContent()
    {
        Rect = new(
            Width: 300f,
            Height: 400f,
            X: App.CurrentWidth / 2f,
            Y: App.CurrentHeight / 3f);

        var (posY, space) = (Rect.Y, 60f);

        foreach (var cmd in Enum.GetValues<ECmd>())
        {
            Buttons.Add(new TextButton()
            {
                Id = cmd,
                Size = 35,
                Text = cmd.ToString(),
                Font = EFont.Romulus,
                Position = new(Rect.X, posY),
            });
            posY += space;
        }

        Title = new Text()
        {
            CharacterSize = 100,
            OutlineThickness = 1f,
            DisplayedString = Global.TITLE,
            FillColor = Factory.Color(EColor.White),
            OutlineColor = Factory.Color(EColor.Black),
            Font = Content.GetResource(EFont.Romulus),
            Position = new Vector2f(App.CurrentWidth / 3f, 25),
        };

        Background = new RectangleShape()
        {
            Size = new(Rect.Width, Rect.Height),
            Position = new(Rect.X - (Rect.Width / 3.5f), Rect.Y - (Rect.Height / 6f)),
            Texture = Content.GetResource(EGraphic.BackgroundHUD).Texture,
        };
    }

    public void LoadEvents()
    {
        foreach (IButton button in Buttons)
        {
            button.LoadEvents();
            button.OnClicked += OnButtonClicked;
        }
    }

    public void Draw(RenderWindow window)
    {
        if (Enabled is false) return;

        window.Draw(Title);
        window.Draw(Background);

        foreach (IButton button in Buttons) button.Draw(window);
    }
    #endregion

    #region Event
    private void OnButtonClicked(object? sender)
    {
        if (Enabled is false) return;

        if (sender is ECmd.NewGame) Global.Invoke(EEvent.Scene, EScene.World);
        if (sender is ECmd.LoadGame) return;
        if (sender is ECmd.Options) return;
        if (sender is ECmd.Quit) Global.Invoke(EEvent.EndGame, null);
    }
    #endregion

    #region Command
    public void SetEnabled(bool value) => Enabled = value;
    #endregion

    #region Dispose
    public void Dispose()
    {
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
