namespace SFMLGame.core.scenes.main;

public sealed class NewGameHUD : IHud
{
    #region Field
    private bool enable;
    #endregion

    #region Property
    private IList<IButton> Buttons { get; } = [];
    private Rect Rect { get; set; } = Rect.Empty;
    private RectangleShape Background { get; set; } = new();

    private string WorldName { get; set; } = string.Empty;
    private string CharacterName { get; set; } = string.Empty;
    private EWorldSize WorldSize { get; set; } = EWorldSize.Tiny;
    private ERace CharacterRace { get; set; } = ERace.Dwarf;
    private EAlignment CharacterAlignment { get; set; } = EAlignment.Neutral;
    private EProfession CharacterProfession { get; set; } = EProfession.Guardian;
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

        #region Character Options
        foreach (var command in Enum.GetValues<ERace>())
        {
            ImageButton imageButton = new(
                id: command,
                image: command,
                position: new(posX, posY))
            {
                Selected = CharacterRace == command,
            };

            posX = imageButton.GetPosition(EDirection.Right);
            Buttons.Add(imageButton);
        }

        posX = Rect.WidthLeft;
        posY += Global.RECT + Rect.VerticalPadding;
        foreach (var command in Enum.GetValues<EAlignment>())
        {
            ImageButton imageButton = new(
                id: command,
                image: command,
                position: new(posX, posY))
            {
                Selected = CharacterAlignment == command,
            };

            posX = imageButton.GetPosition(EDirection.Right);
            Buttons.Add(imageButton);
        }

        posX = Rect.WidthLeft;
        posY += Global.RECT + Rect.VerticalPadding;
        foreach (var command in Enum.GetValues<EProfession>())
        {
            ImageButton imageButton = new(
                id: command,
                image: command,
                position: new(posX, posY))
            {
                Selected = CharacterProfession == command,
            };

            posX = imageButton.GetPosition(EDirection.Right);
            Buttons.Add(imageButton);
        }

        posX = Rect.WidthLeft;
        posY += Global.RECT + Rect.VerticalPadding;

        TextEntry characterEntry = new(
            id: nameof(ERace),
            placeholder: "Entry character name:",
            position: new(posX, posY),
            borderSize: new(Rect.Width - (Rect.HorizontalPadding * 2), 25))
        {
            FontSize = 25,
        };

        Buttons.Add(characterEntry);
        #endregion

        #region World Options
        posX = Rect.WidthLeft;
        posY = Rect.HeightCenter;
        foreach (var command in Enum.GetValues<EWorldSize>())
        {
            TextButton textButton = new(
                id: command,
                text: $"~ Map ~\n{(int)command:D2}x{(int)command:D2}²",
                position: new(posX, posY))
            {
                FontSize = 20,
                Selected = WorldSize == command,
            };

            posX = textButton.GetPosition(EDirection.Right);
            Buttons.Add(textButton);
        }

        posX = Rect.WidthLeft;
        posY += Rect.HorizontalPadding * 2;

        Buttons.Add(new TextEntry(
            id: nameof(EWorldSize),
            placeholder: "Entry world name:",
            position: new(posX, posY),
            borderSize: new(Rect.Width - (Rect.HorizontalPadding * 2), 25f))
        {
            FontSize = 25,
        });
        #endregion

        #region View Options
        Buttons.Add(new ImageButton(
            id: EIcon.Close,
            image: EIcon.Close,
            position: new(Rect.WidthRight, Rect.HeightTop)));

        Buttons.Add(new Button(
            id: EMainMenu.New_Game,
            text: "TO ADVENTURE",
            position: new(Rect.WidthLeft, Rect.HeightBottom - 30),
            borderSize: new(Rect.Width - (Rect.HorizontalPadding * 2), 25))
        {
            FontSize = 23,
            Color = EColor.White,
            BackgroundColor = EColor.DarkSeaGreen,
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
            button.Activated(false);
            button.OnClicked += OnButtonClicked;
            button.OnChanged += OnButtonChanged;
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

            foreach (IButton button in Buttons)
            {
                if (button.Equal(EMainMenu.New_Game))
                    button.Activated(CharacterName.Length > 4 && WorldName.Length > 4);
                else
                    button.Activated(enable);
            }
        });
    }
    #endregion

    #region HUD Event
    public event Action<object?>? OnClicked;

    private void OnButtonChanged(object? sender)
    {
        if (enable is false) return;

        var entry = (TextEntry)Buttons.First(btn => btn.Equal(sender));

        var button = (Button)Buttons.First(btn => btn.Equal(EMainMenu.New_Game));

        if (sender is nameof(EWorldSize))
            WorldName = entry.Text;

        if (sender is nameof(ERace))
            CharacterName = entry.Text;

        button.Activated(CharacterName.Length > 4 && WorldName.Length > 4);
    }

    private void OnButtonClicked(object? sender)
    {
        if (enable is false) return;

        if (sender is EIcon.Close)
        {
            OnClicked?.Invoke(EMainMenu.New_Game);
            return;
        }

        if (sender is EMainMenu.New_Game)
        {
            byte maxRect = (byte)WorldSize;

            List<RegionSchema> regionSchemas = [];

            for (byte row = 0; row < maxRect; row++)
                for (byte column = 0; column < maxRect; column++)
                {
                    regionSchemas.Add(new()
                    {
                        Biome = App.Shuffle(Enum.GetValues<EBiome>()),
                    });
                }

            var worldSchema = new WorldSchema()
            {
                Size = WorldSize,
                Name = WorldName,
                Region = regionSchemas.Select(x => new RegionMetaSchema() { Token = x.Token, Biome = x.Biome }).ToList(),
            };

            var regionSchema = regionSchemas.First();

            var playerSchema = new PlayerSchema()
            {
                Name = CharacterName,
                Race = CharacterRace,
                Alignment = CharacterAlignment,
                Profession = CharacterProfession,
                WorldToken = worldSchema.Token,
                RegionToken = regionSchema.Token,
            };

            FileHandler.SerializeSchema(EFolder.Worlds, worldSchema);

            FileHandler.SerializeSchema(EFolder.Characters, playerSchema);

            regionSchemas.ForEach(schema => { FileHandler.SerializeSchema(EFolder.Regions, schema); });

            Global.Invoke(EEvent.SceneChanged, EScene.World);

            Global.Invoke(EEvent.SchemaChanged, worldSchema);
            Global.Invoke(EEvent.SchemaChanged, playerSchema);
            Global.Invoke(EEvent.SchemaChanged, regionSchema);
            return;
        }

        if (sender is nameof(ERace) or nameof(EWorldSize))
        {
            foreach (var button in Buttons.OfType<TextEntry>())
                button.Selected = button.Id.Equals(sender);
        }

        if (sender is EWorldSize size)
        {
            WorldSize = size;
            foreach (var button in Buttons.OfType<TextButton>().Where(x => x.Id is EWorldSize))
                button.Selected = button.Equal(size);
        }

        if (sender is ERace race)
        {
            CharacterRace = race;
            foreach (var button in Buttons.OfType<ImageButton>().Where(x => x.Id is ERace))
                button.Selected = button.Equal(race);
        }

        if (sender is EAlignment alignment)
        {
            CharacterAlignment = alignment;
            foreach (var button in Buttons.OfType<ImageButton>().Where(x => x.Id is EAlignment))
                button.Selected = button.Equal(alignment);
        }

        if (sender is EProfession profession)
        {
            CharacterProfession = profession;
            foreach (var button in Buttons.OfType<ImageButton>().Where(x => x.Id is EProfession))
                button.Selected = button.Equal(profession);
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
            button.OnChanged -= OnButtonChanged;
            button.Dispose();
        }

        Buttons.Clear();
        Background.Dispose();
    }
    #endregion
}
