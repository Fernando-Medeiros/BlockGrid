namespace SFMLGame.core.scenes.main;

public sealed class ConfigurationHUD : IGameObject
{
    #region Property
    private Text Title { get; set; } = new();
    private bool Enabled { get; set; } = true;
    private IList<IButton> Buttons { get; } = [];
    private Rect Rect { get; set; } = Rect.Empty;
    private RectangleShape Background { get; set; } = new();
    #endregion

    #region Build
    public void LoadContent()
    {
        Rect = new(
            Width: 500f,
            Height: 600f,
            X: App.CurrentWidth / 2f,
            Y: App.CurrentHeight / 3f);
    }

    public void LoadEvents()
    {
    }

    public void Draw(RenderWindow window)
    {
        if (Enabled is false) return;
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
    }
    #endregion
}
