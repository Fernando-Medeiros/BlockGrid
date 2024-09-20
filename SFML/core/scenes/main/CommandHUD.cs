namespace SFMLGame.core.scenes.main;

public sealed class CommandHUD : IGameObject, IDisposable
{
    private enum ECmd : byte { NewGame, LoadGame, Options, Quit }

    private Text Title { get; set; } = new();
    private IList<IButton> Buttons { get; } = [];

    #region Build
    public void LoadContent()
    {
        var (posX, posY, space) = (Global.WINDOW_WIDTH / 2, Global.WINDOW_HEIGHT / 3f, 60f);

        foreach (var cmd in Enum.GetValues<ECmd>())
        {
            Buttons.Add(new TextButton(cmd)
            {
                Size = 35,
                Text = cmd.ToString(),
                Font = EFont.Romulus,
                Position = new(posX, posY),
            });
            posY += space;
        }

        Title = new Text()
        {
            Font = Content.GetResource(EFont.Romulus),
            Position = new Vector2f(Global.WINDOW_WIDTH / 3, 25),
            CharacterSize = 100,
            OutlineThickness = 1f,
            FillColor = Factory.Color(EColor.White),
            OutlineColor = Factory.Color(EColor.Black),
            DisplayedString = Global.TITLE,
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
        window.Draw(Title);

        foreach (IButton button in Buttons) button.Draw(window);
    }
    #endregion

    #region Event
    private void OnButtonClicked(object? sender)
    {
        if (sender is ECmd.NewGame) Global.Invoke(EEvent.Scene, EScene.World);
        if (sender is ECmd.LoadGame) return;
        if (sender is ECmd.Options) return;
        if (sender is ECmd.Quit) Global.Invoke(EEvent.EndGame, null);
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
        foreach (IButton button in Buttons)
        {
            button.OnClicked -= OnButtonClicked;
            button.Dispose();
        }

        Title.Dispose();
        Buttons.Clear();
    }
    #endregion
}
