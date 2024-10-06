namespace SFMLGame.core.scenes.world;

public sealed class DeveloperHUD : IGameObject
{
    private enum EGuide : byte { Biomes, Enemies, Objects }

    private EGuide SelectedGuide { get; set; }
    private ESprite? SelectedEnemy { get; set; }
    private ESprite? SelectedObject { get; set; }
    private Rect Rect { get; set; } = Rect.Empty;
    private IList<IButton> Guides { get; } = [];
    private Dictionary<EGuide, IList<IButton>> Buttons { get; } = [];

    #region Build
    public void LoadContent()
    {
        Rect = new(X: Global.WINDOW_WIDTH - 200, Y: 5f, Width: 200f, Height: 0f);

        var (posX, space) = (Rect.X, 70f);
        foreach (var guide in Enum.GetValues<EGuide>())
        {
            Buttons.Add(guide, []);

            Guides.Add(new TextButton()
            {
                Id = guide,
                Size = 20,
                Font = EFont.Romulus,
                Text = Enum.GetName(guide)!,
                Position = new(posX, Rect.Y),
            });
            posX += space;
        }

        var posY = Rect.Y + 30;
        foreach (var biome in Enum.GetValues<EBiome>())
        {
            Buttons[EGuide.Biomes].Add(new ImageButton()
            {
                Id = biome,
                Image = Factory.Shuffle(biome),
                Position = new(Rect.X, posY),
            });
            posY += Global.RECT + 5;
        }

        posY = Rect.Y + 30;
        foreach (var enemie in new List<ESprite> { ESprite.Aracne, ESprite.Spider })
        {
            Buttons[EGuide.Enemies].Add(new ImageButton()
            {
                Id = enemie,
                Image = enemie,
                Position = new(Rect.X, posY),
            });
            posY += Global.RECT + 5;
        }

        posY = Rect.Y + 30;
        foreach (var gameObject in new List<ESprite> { ESprite.Road })
        {
            Buttons[EGuide.Objects].Add(new ImageButton()
            {
                Id = gameObject,
                Image = gameObject,
                Position = new(Rect.X, posY),
            });
            posY += Global.RECT + 5;
        }
    }

    public void LoadEvents()
    {
        foreach (IButton button in Guides)
        {
            button.LoadEvents();
            button.OnClicked += OnButtonClicked;
        }

        foreach (var guide in Enum.GetValues<EGuide>())
            foreach (var button in Buttons[guide])
            {
                button.LoadEvents();
                button.OnClicked += OnButtonClicked;
            }

        Global.Subscribe(EEvent.Transport, OnSelectedNodeChanged);
    }

    public void Draw(RenderWindow window)
    {
        foreach (IButton button in Guides) button.Draw(window);
        foreach (IButton button in Buttons[SelectedGuide]) button.Draw(window);
    }
    #endregion

    #region Event
    private void OnSelectedNodeChanged(object? sender)
    {
        if (sender is INode2D node)
        {
            if (SelectedGuide is EGuide.Objects && SelectedObject is ESprite sprite)
            {
                if (node.Objects.Any(x => x.Sprite == sprite) is false)
                    node.Objects.Add(Factory.Build(sprite));
            }

            if (SelectedGuide is EGuide.Enemies && node.Body is null)
                node.SetBody(Factory.Build(EBody.Enemy, node));
        }
    }

    private void OnButtonClicked(object? sender)
    {
        SelectedObject = null;
        SelectedEnemy = null;

        switch (SelectedGuide, sender)
        {
            case (_, EGuide guide): SelectedGuide = guide; break;
            case (EGuide.Objects, ESprite sprite): SelectedObject = sprite; break;
            case (EGuide.Enemies, ESprite sprite): SelectedEnemy = sprite; break;
            case (EGuide.Biomes, EBiome biome): Global.Invoke(EEvent.Transport, biome); break;
            default: break;
        }

        if (sender is EGuide)
        {
            foreach (var guide in Enum.GetValues<EGuide>())
                foreach (var button in Buttons[guide])
                    button.IsEnabled(guide == SelectedGuide);
        }
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
        Global.UnSubscribe(EEvent.Transport, OnSelectedNodeChanged);

        foreach (IButton button in Guides)
        {
            button.OnClicked -= OnButtonClicked;
            button.Dispose();
        }

        foreach (var guide in Enum.GetValues<EGuide>())
            foreach (var button in Buttons[guide])
            {
                button.OnClicked -= OnButtonClicked;
                button.Dispose();
            }

        Guides.Clear();
        Buttons.Clear();
    }
    #endregion
}
