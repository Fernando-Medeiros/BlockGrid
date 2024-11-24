namespace SFMLGame.core.scenes.world;

public sealed class DeveloperHUD : IView
{
    private enum EGuide : byte { Enemies, Objects }

    private EGuide SelectedGuide { get; set; }
    private ESprite? SelectedEnemy { get; set; }
    private ESprite? SelectedObject { get; set; }
    private Rect Rect { get; set; } = Rect.Empty;
    private IList<IButton> Guides { get; } = [];
    private Dictionary<EGuide, IList<IButton>> Buttons { get; } = [];

    #region Build
    public void Build()
    {
        var (width, _) = App.Configuration.WindowResolution;

        Rect = new(x: width - 200, y: 5f, width: 200f, height: 0f);

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

    public void Event()
    {
        foreach (IButton button in Guides)
        {
            button.Event();
            button.OnClicked += OnButtonClicked;
        }

        foreach (var guide in Enum.GetValues<EGuide>())
            foreach (var button in Buttons[guide])
            {
                button.Event();
                button.OnClicked += OnButtonClicked;
            }

        Global.Subscribe(EEvent.NodeChanged, OnSelectedNodeChanged);
    }

    public void Render(RenderWindow window)
    {
        foreach (IButton button in Guides) button.Render(window);
        foreach (IButton button in Buttons[SelectedGuide]) button.Render(window);
    }
    #endregion

    #region Event
    private void OnSelectedNodeChanged(object? sender)
    {
        if (sender is INode2D node)
        {
            if (SelectedGuide is EGuide.Objects && SelectedObject is ESprite sprite)
            {
                if (node.Items2D.Any(x => x.Image.Equals(sprite)) is false)
                    node.Items2D.Add(Factory.Build(sprite));
            }

            if (SelectedGuide is EGuide.Enemies && node.Body2D is null)
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
            default: break;
        }

        if (sender is EGuide)
        {
            foreach (var guide in Enum.GetValues<EGuide>())
                foreach (var button in Buttons[guide])
                    button.SetActivated(guide == SelectedGuide);
        }
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
        Global.Unsubscribe(EEvent.NodeChanged, OnSelectedNodeChanged);

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
