namespace SFMLGame.core.scenes.world;

public sealed class CommandHUD : IView, IDisposable
{
    private IList<IButton> Buttons { get; } = [];
    private Rect Rect { get; set; } = Rect.Empty;

    #region Build
    public void Build()
    {
        var (width, height) = App.Configuration.WindowResolution;

        Rect = new(x: width - Global.RECT - 5, y: height - 5, width: 0f, height: 0f);

        var (posY, space) = (Rect.Y, 5f);

        EIcon[] icons = [EIcon.Exit, EIcon.ZoomOut, EIcon.ZoomIn];

        foreach (EIcon icon in icons)
        {
            posY -= Global.RECT + space;

            Buttons.Add(new ImageButton()
            {
                Id = icon,
                Image = icon,
                Position = new(Rect.X, posY),
            });
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
