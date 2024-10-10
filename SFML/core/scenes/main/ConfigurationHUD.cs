namespace SFMLGame.core.scenes.main;

public sealed class ConfigurationHUD : IGameObject
{
    private enum ECommand : byte { Music_Volume, Sound_Volume, FPS }

    #region Property
    private IList<Text> Guides { get; } = [];
    private IList<IButton> Buttons { get; } = [];
    private Rect Rect { get; set; } = Rect.Empty;
    private RectangleShape Background { get; set; } = new();
    #endregion

    #region Build
    public void LoadContent()
    {
        Rect = new(
            Width: 500f,
            Height: 600f,
            X: App.CurrentWidth / 2f,
            Y: App.CurrentHeight / 3f);

        foreach (var command in Enum.GetValues<ECommand>())
        {
            var gap = (byte)command * 70f;

            var text = command.ToString().Replace("_", " ");

            Guides.Add(new Text()
            {
                CharacterSize = 25,
                DisplayedString = text,
                FillColor = Factory.Color(EColor.White),
                Font = Content.GetResource(EFont.Romulus),
                Position = new(Rect.X, Rect.Y + gap),
            });
        }

        Background = new RectangleShape()
        {
            Size = new(Rect.Width, Rect.Height),
            Position = new(Rect.X - (Rect.Width / 3.5f), Rect.Y - (Rect.Height / 6f)),
            Texture = Content.GetResource(EGraphic.BackgroundHUD).Texture,
        };
    }

    public void LoadEvents()
    {
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(Background);

        foreach (Text guide in Guides) window.Draw(guide);
        foreach (IButton button in Buttons) button.Draw(window);
    }
    #endregion

    #region Event
    #endregion

    #region Dispose
    public void Dispose()
    {
        foreach (Text guide in Guides)
        {
            guide.Dispose();
        }
        foreach (IButton button in Buttons)
        {
            button.Dispose();
        }

        Guides.Clear();
        Buttons.Clear();
        Background.Dispose();
    }
    #endregion
}
