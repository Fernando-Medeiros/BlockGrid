namespace SFMLGame.core.scenes.main;

public sealed class ConfigurationHUD : IGameObject
{
    private enum ECommand : byte { Music_Volume, Sound_Volume, FPS, Language }

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

        int count = 0;

        foreach (var command in Enum.GetValues<ECommand>())
        {
            var offset = count * 70f;

            var text = command.ToString().Replace("_", " ");

            Guides.Add(new Text()
            {
                CharacterSize = 25,
                DisplayedString = text,
                FillColor = Factory.Color(EColor.White),
                Font = Content.GetResource(EFont.Romulus),
                Position = new(Rect.X, Rect.Y + offset),
            });
            count++;
        }

        count = 0;

        foreach (var command in Enum.GetValues<EVolume>())
        {
            var offset = count * 40f;

            Buttons.Add(new TextButton()
            {
                Size = 25,
                Text = $"{(byte)command}",
                Id = (ECommand.Music_Volume, command),
                Position = new(Rect.X + offset, Rect.Y + 30f),
            });
            count++;
        }

        count = 0;

        foreach (var command in Enum.GetValues<EVolume>())
        {
            var offset = count * 40f;

            Buttons.Add(new TextButton()
            {
                Size = 25,
                Text = $"{(byte)command}",
                Id = (ECommand.Sound_Volume, command),
                Position = new(Rect.X + offset, Rect.Y + 100f),
            });
            count++;
        }

        count = 0;

        foreach (var command in Enum.GetValues<EFrame>())
        {
            var offset = count * 40f;

            Buttons.Add(new TextButton()
            {
                Size = 25,
                Id = command,
                Text = $"{(byte)command}",
                Position = new(Rect.X + offset, Rect.Y + 170f),
            });
            count++;
        }

        count = 0;

        foreach (var command in Enum.GetValues<ELanguage>())
        {
            var offset = count * 40f;

            var text = command.ToString().Replace("_", "-");

            Buttons.Add(new TextButton()
            {
                Size = 25,
                Text = text,
                Id = command,
                Position = new(Rect.X + offset, Rect.Y + 240f),
            });
            count++;
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
        foreach (IButton button in Buttons)
        {
            button.LoadEvents();
            button.OnClicked += OnButtonClicked;
        }
    }

    public void Draw(RenderWindow window)
    {
        window.Draw(Background);

        foreach (Text guide in Guides) window.Draw(guide);

        foreach (IButton button in Buttons) button.Draw(window);
    }
    #endregion

    #region Event
    private void OnButtonClicked(object? sender)
    {
        if (sender is EFrame frame)
            App.CurrentFrame = (byte)frame;

        if (sender is ELanguage language)
            App.CurrentLanguage = language;

        if (sender is (ECommand.Sound_Volume, EVolume sound))
            App.CurrentSoundVolume = (byte)sound;

        if (sender is (ECommand.Music_Volume, EVolume soundtrack))
            App.CurrentSoundtrackVolume = (byte)soundtrack;
    }
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
            button.OnClicked -= OnButtonClicked;
            button.Dispose();
        }

        Guides.Clear();
        Buttons.Clear();
        Background.Dispose();
    }
    #endregion
}
