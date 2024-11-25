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
        Rect = new Rect()
          .WithSize(width: 200f, height: 200f)
          .WithPadding(vertical: 0f, horizontal: 20f)
          .WithAlignment(EDirection.TopRight);

        float posX = Rect.WidthLeft;

        foreach (var guide in Enum.GetValues<EGuide>())
        {
            Buttons.Add(guide, []);

            TextButton textButton = new(
                id: guide,
                text: Enum.GetName(guide),
                position: new(posX, Rect.HeightTop))
            {
                FontSize = 20,
            };

            posX = textButton.GetPosition(EDirection.Right);

            Guides.Add(textButton);
        }

        float posY = Rect.HeightTop + Rect.HorizontalPadding;

        foreach (var enemie in new List<ESprite> { ESprite.Aracne, ESprite.Spider })
        {
            ImageButton imageButton = new(
                id: enemie,
                image: enemie,
                position: new(Rect.WidthLeft, posY));

            posY = imageButton.GetPosition(EDirection.Bottom);

            Buttons[EGuide.Enemies].Add(imageButton);
        }

        posY = Rect.HeightTop + Rect.HorizontalPadding;

        foreach (var gameObject in new List<ESprite> { ESprite.Road })
        {
            ImageButton imageButton = new(
                 id: gameObject,
                 image: gameObject,
                 position: new(Rect.WidthLeft, posY));

            posY = imageButton.GetPosition(EDirection.Bottom);

            Buttons[EGuide.Objects].Add(imageButton);
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
                    button.Activated(guide == SelectedGuide);
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
