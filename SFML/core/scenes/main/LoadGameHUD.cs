namespace SFMLGame.core.scenes.main;

public sealed class LoadGameHUD : IHud
{
    #region Field
    private bool enable;
    #endregion

    #region Property
    private IList<IButton> Buttons { get; } = [];
    private IList<PlayerSchema> Characters { get; } = [];
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

        foreach (var fileName in names)
        {
            Characters.Add(FileHandler.DeserializeSchema<PlayerSchema>(EFolder.Characters, fileName));
        }

        float posY = Rect.HeightTop;

        #region Characters
        foreach (var schema in Characters.OrderByDescending(x => x.UpdatedOn))
        {
            TextButton textButton = new(
                id: schema.Token,
                text: schema.ToString(),
                position: new(Rect.WidthLeft, posY))
            {
                Size = 20,
            };

            ImageButton imageButton = new()
            {
                Id = (EIcon.Delete, schema.Token),
                Image = EIcon.Delete,
                Position = new(Rect.WidthRight - Rect.Padding, posY)
            };

            posY = textButton.GetPosition(EDirection.Bottom);

            Buttons.Add(textButton);
            Buttons.Add(imageButton);
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
            button.SetActivated(false);
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
        Task.Run(async () =>
        {
            await Task.Delay(Global.VIEW_DELAY);
            enable = !enable;
            foreach (IButton button in Buttons) button.SetActivated(enable);
        });
    }
    #endregion

    #region Event
    public event Action<object?>? OnClicked;

    private void OnButtonClicked(object? sender)
    {
        if (enable is false) return;

        if (sender is EIcon.Close)
        {
            OnClicked?.Invoke(EMainMenu.Load_Game);
            return;
        }

        // Deleta os arquivos vinculados ao personagem e remove os dados em memoria;
        if (sender is (EIcon, string))
        {
            (EIcon, string token) tuple = ((EIcon, string))sender;

            var playerSchema = Characters.First(x => x.Token == tuple.token);

            var worldSchema = FileHandler.DeserializeSchema<WorldSchema>(EFolder.Worlds, playerSchema.WorldToken);

            var buttons = Buttons.Where(x => x.Equal(sender) || x.Equal(tuple.token)).ToList();

            foreach (var button in buttons)
            {
                button.OnClicked -= OnButtonClicked;
                button.Dispose();
            }

            buttons.ForEach(x => Buttons.Remove(x));

            Characters.Remove(playerSchema);

            foreach (var regionMetaSchema in worldSchema.Region)
                File.Delete($"{FileHandler.MainFolder}/{EFolder.Regions}/{regionMetaSchema.Token}.xml");

            File.Delete($"{FileHandler.MainFolder}/{EFolder.Worlds}/{worldSchema.Token}.xml");
            File.Delete($"{FileHandler.MainFolder}/{EFolder.Characters}/{playerSchema.Token}.xml");
            return;
        }

        // Carrega os dados do personagem/mundo/região e inicia a cena do mundo;
        if (sender is string token)
        {
            var playerSchema = Characters.First(x => x.Token == token);

            var worldSchema = FileHandler.DeserializeSchema<WorldSchema>(EFolder.Worlds, playerSchema.WorldToken);

            var regionSchema = FileHandler.DeserializeSchema<RegionSchema>(EFolder.Regions, playerSchema.RegionToken);

            // Inicia a construção da cena e as assinaturas dos enventos para enviar os schemas em seguida;
            Global.Invoke(EEvent.SceneChanged, EScene.World);

            Global.Invoke(EEvent.SchemaChanged, playerSchema);
            Global.Invoke(EEvent.SchemaChanged, worldSchema);
            Global.Invoke(EEvent.SchemaChanged, regionSchema);
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
