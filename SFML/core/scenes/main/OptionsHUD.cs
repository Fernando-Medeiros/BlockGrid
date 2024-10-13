namespace SFMLGame.core.scenes.main;

public sealed class OptionsHUD : IHud
{
    private enum ECommand : byte
    {
        Music_Volume,
        Sound_Volume,
        FPS,
        Language
    }

    #region Field
    private bool enable;
    #endregion

    #region Property
    private IList<Text> Guides { get; } = [];
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
                Font = core.Content.GetResource(EFont.Romulus),
                Position = new(Rect.X, Rect.Y + offset),
            });
            count++;
        }

        count = 0;

        foreach (var command in Enum.GetValues<EMusicVolume>())
        {
            var offset = count * 40f;

            Buttons.Add(new TextButton()
            {
                Size = 25,
                Id = command,
                Text = $"{(byte)command}",
                Position = new(Rect.X + offset, Rect.Y + 30f),

                Color = App.CurrentMusicVolume == (byte)command ? EColor.CornFlowerBlue : EColor.White,
            });
            count++;
        }

        count = 0;

        foreach (var command in Enum.GetValues<ESoundVolume>())
        {
            var offset = count * 40f;

            Buttons.Add(new TextButton()
            {
                Size = 25,
                Id = command,
                Text = $"{(byte)command}",
                Position = new(Rect.X + offset, Rect.Y + 100f),

                Color = App.CurrentSoundVolume == (byte)command ? EColor.CornFlowerBlue : EColor.White,
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

                Color = App.CurrentFrame == (byte)command ? EColor.CornFlowerBlue : EColor.White,
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

                Color = App.CurrentLanguage == command ? EColor.CornFlowerBlue : EColor.White,
            });
            count++;
        }

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

        foreach (Text guide in Guides) window.Draw(guide);

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
            OnClicked?.Invoke(EMainMenu.Options);

        if (sender is EFrame frame)
            App.CurrentFrame = (byte)frame;

        if (sender is ELanguage language)
            App.CurrentLanguage = language;

        if (sender is ESoundVolume soundVolume)
            App.CurrentSoundVolume = (byte)soundVolume;

        if (sender is EMusicVolume musicVolume)
            App.CurrentMusicVolume = (byte)musicVolume;


        foreach (var button in Buttons.OfType<TextButton>())
        {
            if (button.Id.Equals(sender))
                button.Color = EColor.CornFlowerBlue;

            else if (button.Id.GetType() == sender?.GetType())
                button.Color = EColor.White;
        }
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
        OnClicked = null;

        foreach (Text guide in Guides) guide.Dispose();

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
