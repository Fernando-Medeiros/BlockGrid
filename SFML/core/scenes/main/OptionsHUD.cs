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
          .WithSize(width: 500f, height: 700f, padding: 68f)
          .WithAlignment();

        int count = 0;

        foreach (var command in Enum.GetValues<ECommand>())
        {
            var offset = count * 70f;

            Guides.Add(new Text()
            {
                CharacterSize = 25,
                DisplayedString = command.ToString().Replace("_", " "),
                FillColor = Factory.Color(EColor.White),
                Font = Content.GetResource<Font>(EFont.Romulus),
                Position = new(Rect.WidthLeft, Rect.HeightTop + offset),
            });
            count++;
        }

        count = 0;

        foreach (var command in Enum.GetValues<EMusicVolume>())
        {
            var offset = count * 40f;

            Buttons.Add(new TextButton(
                id: command,
                text: $"{(byte)command}",
                position: new(Rect.WidthLeft + offset, Rect.HeightTop + 30f))
            {
                FontSize = 20,
                Selected = App.Configuration.MusicVolume == (byte)command
            });
            count++;
        }

        count = 0;

        foreach (var command in Enum.GetValues<ESoundVolume>())
        {
            var offset = count * 40f;

            Buttons.Add(new TextButton(
                id: command,
                text: $"{(byte)command}",
                position: new(Rect.WidthLeft + offset, Rect.HeightTop + 100f))
            {
                FontSize = 20,
                Selected = App.Configuration.SoundVolume == (byte)command,
            });
            count++;
        }

        //count = 0;

        //foreach (var command in Enum.GetValues<EWindowMode>())
        //{
        //    var offset = count * 80f;

        //    Buttons.Add(new TextButton()
        //    {
        //        Size = 20,
        //        Id = command,
        //        Text = $"{command}",
        //        Position = new(Rect.WidthLeft + offset, Rect.HeightTop + 170f),
        //        Selected = App.Configuration.WindowMode == (byte)command,
        //    });
        //    count++;
        //}

        //count = 0;

        //foreach (var command in Enum.GetValues<EWindowResolution>())
        //{
        //    var offset = count * 90f;

        //    Buttons.Add(new TextButton()
        //    {
        //        Size = 20,
        //        Id = command,
        //        Text = command.ToString().Replace("R_", ""),
        //        Position = new(Rect.WidthLeft + offset, Rect.HeightTop + 240f),
        //        Selected = App.Configuration.WindowResolution == Factory.Resolution(command)
        //    });
        //    count++;
        //}

        count = 0;

        foreach (var command in Enum.GetValues<EFrame>())
        {
            var offset = count * 40f;

            Buttons.Add(new TextButton(
                id: command,
                text: $"{(byte)command}",
                position: new(Rect.WidthLeft + offset, Rect.HeightTop + 310f))
            {
                FontSize = 20,
                Selected = App.Configuration.Frame == (byte)command,
            });
            count++;
        }

        count = 0;

        foreach (var command in Enum.GetValues<ELanguage>())
        {
            var offset = count * 40f;

            Buttons.Add(new TextButton(
                id: command,
                text: command.ToString().Replace("_", "-"),
                position: new(Rect.WidthLeft + offset, Rect.HeightTop + 370f))
            {
                FontSize = 20,
                Selected = App.Configuration.Language == command,
            });
            count++;
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
