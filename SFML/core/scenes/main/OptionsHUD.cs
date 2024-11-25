namespace SFMLGame.core.scenes.main;

public sealed class OptionsHUD : IHud
{
    private enum ECommand : byte
    {
        Music_Volume,
        Sound_Volume,
        Window_Mode,
        Resolution,
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
        Rect = new Rect()
          .WithSize(width: 500f, height: 500f)
          .WithPadding(vertical: 10f, horizontal: 32f)
          .WithAlignment();

        float posY = Rect.HeightTop;
        float posX = Rect.WidthLeft;

        Dictionary<ECommand, float> verticalPositions = [];

        foreach (var command in Enum.GetValues<ECommand>())
        {
            Text subtitle = new()
            {
                CharacterSize = 25,
                FillColor = Factory.Color(EColor.White),
                Font = Content.GetResource<Font>(EFont.Romulus),
                DisplayedString = command.ToString().Replace("_", " "),
                Position = new(posX, posY),
            };

            FloatRect rect = subtitle.GetGlobalBounds();
            posY = rect.Position.Y + rect.Height + Rect.HorizontalPadding;
            verticalPositions[command] = rect.Position.Y + rect.Height;

            Guides.Add(subtitle);
        }

        posX = Rect.WidthLeft;
        foreach (var command in Enum.GetValues<EMusicVolume>())
        {
            TextButton textButton = new(
                id: command,
                text: $"{(byte)command}",
                position: new(posX, verticalPositions[ECommand.Music_Volume]))
            {
                FontSize = 20,
                Selected = App.Configuration.MusicVolume == (byte)command
            };

            Buttons.Add(textButton);
            posX = textButton.GetPosition(EDirection.Right);
        }

        posX = Rect.WidthLeft;
        foreach (var command in Enum.GetValues<ESoundVolume>())
        {
            TextButton textButton = new(
              id: command,
              text: $"{(byte)command}",
              position: new(posX, verticalPositions[ECommand.Sound_Volume]))
            {
                FontSize = 20,
                Selected = App.Configuration.SoundVolume == (byte)command,
            };

            Buttons.Add(textButton);
            posX = textButton.GetPosition(EDirection.Right);
        }

        posX = Rect.WidthLeft;
        foreach (var command in Enum.GetValues<EWindowMode>())
        {
            TextButton textButton = new(
                 id: command,
                 text: Enum.GetName(command),
                 position: new(posX, verticalPositions[ECommand.Window_Mode]))
            {
                FontSize = 20,
                Selected = App.Configuration.WindowMode == (byte)command,
            };

            Buttons.Add(textButton);
            posX = textButton.GetPosition(EDirection.Right);
        }

        posX = Rect.WidthLeft;
        foreach (var command in Enum.GetValues<EWindowResolution>())
        {
            TextButton textButton = new(
                id: command,
                text: command.ToString().Replace("R_", ""),
                position: new(posX, verticalPositions[ECommand.Resolution]))
            {
                FontSize = 20,
                Selected = App.Configuration.WindowResolution == Factory.Resolution(command)
            };

            Buttons.Add(textButton);
            posX = textButton.GetPosition(EDirection.Right);
        }

        posX = Rect.WidthLeft;
        foreach (var command in Enum.GetValues<EFrame>())
        {
            TextButton textButton = new(
                 id: command,
                 text: $"{(byte)command}",
                 position: new(posX, verticalPositions[ECommand.FPS]))
            {
                FontSize = 20,
                Selected = App.Configuration.Frame == (byte)command,
            };

            Buttons.Add(textButton);
            posX = textButton.GetPosition(EDirection.Right);
        }

        posX = Rect.WidthLeft;
        foreach (var command in Enum.GetValues<ELanguage>())
        {
            TextButton textButton = new(
                 id: command,
                 text: command.ToString().Replace("_", "-"),
                 position: new(posX, verticalPositions[ECommand.Language]))
            {
                FontSize = 20,
                Selected = App.Configuration.Language == command,
            };

            Buttons.Add(textButton);
            posX = textButton.GetPosition(EDirection.Right);
        }

        Buttons.Add(new ImageButton(
            id: EIcon.Close,
            image: EIcon.Close,
            position: new(Rect.WidthRight, Rect.HeightTop)));

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
            button.Activated(false);
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
        Task.Run(async () =>
        {
            await Task.Delay(Global.VIEW_DELAY);
            enable = !enable;
            foreach (IButton button in Buttons) button.Activated(enable);
        });
    }
    #endregion

    #region Event
    public event Action<object?>? OnClicked;

    private void OnButtonClicked(object? sender)
    {
        if (sender is EIcon.Close)
        {
            FileHandler.SerializeSchema(EFolder.Options, App.Configuration);

            OnClicked?.Invoke(EMainMenu.Options);
        }

        if (sender is EFrame frame)
            App.Configuration.Frame = (byte)frame;

        if (sender is ELanguage language)
            App.Configuration.Language = language;

        if (sender is ESoundVolume soundVolume)
            App.Configuration.SoundVolume = (byte)soundVolume;

        if (sender is EMusicVolume musicVolume)
            App.Configuration.MusicVolume = (byte)musicVolume;

        if (sender is EWindowMode windowMode)
            App.Configuration.WindowMode = (byte)windowMode;

        if (sender is EWindowResolution windowResolution)
            App.Configuration.WindowResolution = Factory.Resolution(windowResolution);

        foreach (var button in Buttons.OfType<TextButton>().Where(x => x.Id.GetType() == sender?.GetType()))
        {
            button.Selected = button.Id.Equals(sender);
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
