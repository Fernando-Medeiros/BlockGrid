namespace SFMLGame.core.scenes.main;

public sealed class LoadGameHUD : IHud
{
    #region Field
    private bool enable;
    #endregion

    #region Property
    private IList<PlayerSchema> Characters { get; } = [];

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

        var names = Directory.GetFiles($"{FileHandler.MainFolder}/{EFolder.Characters}")
            .Select(Path.GetFileNameWithoutExtension)
            .ToList();

        foreach (string name in names)
        {
            Characters.Add(FileHandler.DeserializeSchema<PlayerSchema>(EFolder.Characters, name));
        }

        float offset = 0f;

        #region Worlds
        foreach (var schema in Characters)
        {
            Buttons.Add(new TextButton()
            {
                Size = 20,
                Id = schema.Token,
                Text = schema.ToString(),
                Position = new(Rect.WidthLeft, Rect.HeightTop + offset)
            });
            offset += 30;
        }
        #endregion

        #region View Options
        Buttons.Add(new ImageButton()
        {
            Id = EIcon.Close,
            Image = EIcon.Close,
            Position = new(Rect.WidthRight, Rect.HeightTop),
        });

        Background = new RectangleShape()
        {
            Position = new(Rect.X, Rect.Y),
            Size = new(Rect.Width, Rect.Height),
            Texture = Content.GetResource<Sprite>(EGraphic.BackgroundHUD).Texture,
        };
        #endregion
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
        if (sender is EIcon.Close)
        {
            OnClicked?.Invoke(EMainMenu.Load_Game);
            return;
        }

        if (sender is string token)
        {
            var player = Characters.First(x => x.Token == token);
            var world = FileHandler.DeserializeSchema<WorldSchema>(EFolder.Worlds, player.WorldToken);
            var region = FileHandler.DeserializeSchema<RegionSchema>(EFolder.Regions, player.RegionToken);

            Global.Invoke(EEvent.SceneChanged, EScene.World);

            Global.Invoke(EEvent.SchemaChanged, player);
            Global.Invoke(EEvent.SchemaChanged, world);
            Global.Invoke(EEvent.SchemaChanged, region);
        }
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

        Characters.Clear();
        Buttons.Clear();
        Background.Dispose();
    }
    #endregion
}
