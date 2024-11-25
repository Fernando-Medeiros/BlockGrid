namespace SFMLGame.core.scenes.world;

public sealed class CommandHUD : IView, IDisposable
{
    private IList<IButton> Buttons { get; } = [];
    private Rect Rect { get; set; } = Rect.Empty;

    #region Build
    public void Build()
    {
        EIcon[] icons = [EIcon.ZoomIn, EIcon.ZoomOut, EIcon.Exit];

        Rect = new Rect()
          .WithSize(width: 50f, height: 120f)
          .WithPadding(vertical: 10f, horizontal: 10f)
          .WithAlignment(EDirection.BottomRight);

        float posY = Rect.HeightTop;

        foreach (EIcon icon in icons)
        {
            ImageButton imageButton = new(
                id: icon,
                image: icon,
                position: new(Rect.WidthLeft, posY));

            posY = imageButton.GetPosition(EDirection.Bottom);

            Buttons.Add(imageButton);
        }
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
        foreach (IButton button in Buttons) button.Render(window);
    }
    #endregion

    #region Event
    private void OnButtonClicked(object? sender)
    {
        if (sender is EIcon.Exit)
        {
            Global.Invoke(EEvent.SaveGameChanged, null);
            Global.Invoke(EEvent.SceneChanged, EScene.Main);
        };

        if (sender is EIcon.ZoomIn) Global.Invoke(EEvent.KeyPressed, Key.Z);
        if (sender is EIcon.ZoomOut) Global.Invoke(EEvent.KeyPressed, Key.X);
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
        foreach (IButton button in Buttons)
        {
            button.OnClicked -= OnButtonClicked;
            button.Dispose();
        }

        Buttons.Clear();
    }
    #endregion
}
