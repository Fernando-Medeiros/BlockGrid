namespace SFMLGame.core.scenes.world;

public sealed class DeveloperHUD : IGameObject
{
    private ESprite? SelectedSprite { get; set; }
    private IList<IButton> Buttons { get; } = [];
    private Rect Rect { get; set; } = Rect.Empty;

    #region Build
    public void LoadContent()
    {
        Rect = new(X: Global.WINDOW_WIDTH - 130, Y: 5f, Width: 130f, Height: 0f);

        var (posY, space) = (Rect.Y, 20f);

        foreach (EBiome biome in Enum.GetValues<EBiome>())
        {
            Buttons.Add(new TextButton()
            {
                Id = biome,
                Size = 20,
                Font = EFont.Romulus,
                Position = new(Rect.X, posY),
                Text = Enum.GetName(biome)!,
            });
            posY += space;
        }

        posY += space;

        foreach (ESprite sprite in new List<ESprite> { ESprite.Aracne, ESprite.Spider, ESprite.Road })
        {
            Buttons.Add(new ImageButton()
            {
                Id = sprite,
                Image = sprite,
                Position = new(Rect.X, posY),
            });
            posY += Global.RECT + 5;
        }
    }

    public void LoadEvents()
    {
        foreach (IButton button in Buttons)
        {
            button.LoadEvents();
            button.OnClicked += OnButtonClicked;
        }

        Global.Subscribe(EEvent.Transport, OnSelectedNodeChanged);
    }

    public void Draw(RenderWindow window)
    {
        foreach (IButton button in Buttons) button.Draw(window);
    }
    #endregion

    #region Event
    private void OnSelectedNodeChanged(object? sender)
    {
        if (SelectedSprite is not null && sender is INode2D node)
        {
            if (SelectedSprite is ESprite.Road)
            {
                if (node.Objects.Any(x => x.Sprite == ESprite.Road) is false)
                    node.Objects.Add(Factory.Build(ESprite.Road));
                return;
            }

            if (node.Body is null)
                node.SetBody(Factory.Build(EBody.Enemy, node));
        }
    }

    private void OnButtonClicked(object? sender)
    {
        switch (sender)
        {
            case ESprite sprite: SelectedSprite = sprite; break;
            case EBiome biome: Global.Invoke(EEvent.Transport, biome); break;
            default: SelectedSprite = null; break;
        }
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
        Global.UnSubscribe(EEvent.Transport, OnSelectedNodeChanged);

        foreach (IButton button in Buttons)
        {
            button.OnClicked -= OnButtonClicked;
            button.Dispose();
        }

        Buttons.Clear();
    }
    #endregion
}
