namespace SFMLGame.core.scenes.main;

public sealed class CommandHUD : IGameObject, IDisposable
{
    private enum ECommand : byte { New_Game, Load_Game, Options, Quit }

    #region Property
    private Text Title { get; set; } = new();
    private IList<IButton> Buttons { get; } = [];
    private Rect Rect { get; set; } = Rect.Empty;
    private RectangleShape Background { get; set; } = new();
    #endregion

    #region Build
    public void LoadContent()
    {
        Rect = new(
            Width: 300f,
            Height: 500f,
            X: 100f,
            Y: 100f);

        foreach (var command in Enum.GetValues<ECommand>())
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
            Font = Content.GetResource(EFont.Romulus),
            Position = new(Rect.X - 40, Rect.Y - (Rect.Height / 10f)),
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
        window.Draw(Background);
        window.Draw(Title);

        foreach (IButton button in Buttons) button.Draw(window);
    }
    #endregion

    #region Event
    private void OnButtonClicked(object? sender)
    {
        if (sender is ECommand.New_Game) Global.Invoke(EEvent.Scene, EScene.World);
        if (sender is ECommand.Load_Game) return;
        if (sender is ECommand.Options) return;
        if (sender is ECommand.Quit) Global.Invoke(EEvent.EndGame, null);
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

        Buttons.Clear();

        Title.Dispose();
        Background.Dispose();
    }
    #endregion
}
