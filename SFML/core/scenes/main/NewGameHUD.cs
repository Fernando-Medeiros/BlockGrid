namespace SFMLGame.core.scenes.main;

public sealed class NewGameHUD : IHud
{
    #region Field
    private bool enable;
    #endregion

    #region Property
    private EWorldSize WorldSize { get; set; } = EWorldSize.Small;
    private IList<IList<(ETerrain, Position2D)>> Collection { get; } = [];

    private IList<IButton> Buttons { get; } = [];
    private Rect Rect { get; set; } = Rect.Empty;
    private RectangleShape Background { get; set; } = new();
    #endregion

    #region Build
    public void Build()
    {
        Rect = new Rect()
            .WithSize(width: 650f, height: 950f, padding: 68f)
            .WithAlignment();

        var offset = 0f;
        foreach (var command in Enum.GetValues<EWorldSize>())
        {
            Buttons.Add(new TextButton()
            {
                Size = 20,
                Id = command,
                Text = $"{(byte)command}x{(byte)command}",
                Position = new(Rect.WidthLeft, Rect.HeightTop + offset),
            });
            offset += 25f;
        }

        Buttons.Add(new TextEntry()
        {
            Size = 25,
            Text = "World name..",
            Id = EEvent.TextEntered,
            Position = new(Rect.WidthLeft, Rect.HeightBottom - 32),
        });

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
            Texture = Content.GetResource(EGraphic.BackgroundHUD).Texture,
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

        foreach (IButton button in Buttons) button.Render(window);

        for (byte row = 0; row < Collection.Count; row++)
            for (byte column = 0; column < Collection[row].Count; column++)
            {
                var (resource, position) = Collection[row][column];
                var sprite = Content.GetResource(resource);
                sprite.Position = position;
                sprite.Color = Factory.Color(EOpacity.Light);
                window.Draw(sprite);
            }
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

    private void OnWorldChanged(EWorldSize worldSize)
    {
        Collection.Clear();
        WorldSize = worldSize;

        var biomes = Enum.GetValues<EBiome>();

        for (byte row = 0; row < (byte)WorldSize; row++)
        {
            Collection.Add([]);

            for (byte column = 0; column < (byte)WorldSize; column++)
            {
                var position2D = new Position2D(row, column, Rect.WidthLeft + (column * Global.RECT), Rect.HeightTop + 60f + (row * Global.RECT));
                var sprite = Factory.Shuffle(App.Shuffle(biomes));

                Collection.ElementAt(row).Add((sprite, position2D));
            }
        }
    }

    private void OnButtonClicked(object? sender)
    {
        if (sender is EIcon.Close)
            OnClicked?.Invoke(EMainMenu.New_Game);

        if (sender is EWorldSize worldSize)
            OnWorldChanged(worldSize);

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

        foreach (IButton button in Buttons)
        {
            button.OnClicked -= OnButtonClicked;
            button.Dispose();
        }

        Buttons.Clear();
        Background.Dispose();
    }
    #endregion
}
