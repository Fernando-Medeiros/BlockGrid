namespace SFMLGame.core.scenes.main;

public sealed class MainScene : View, IGameObject, IDisposable
{
    private FloatRect ViewRect { get; init; }
    private IList<IGameObject> Collection { get; } = [];

    public MainScene(FloatRect viewRect) : base(viewRect)
    {
        ViewRect = viewRect;

        Global.Subscribe(EEvent.Scene, (sender) =>
        {
            if (sender is EScene.Main)
            {
                LoadContent();
                LoadEvents();
                return;
            }

            Dispose();
        });
    }

    #region Build
    public void LoadContent()
    {
        Collection.Add(new BackgroundView());
        Collection.Add(new CommandHUD());

        foreach (var gameObject in Collection) gameObject.LoadContent();
    }

    public void LoadEvents()
    {
        foreach (var gameObject in Collection) gameObject.LoadEvents();
    }

    public void Draw(RenderWindow window)
    {
        window.SetView(this);

        foreach (var gameObject in Collection) gameObject.Draw(window);
    }
    #endregion

    #region Dispose
    public new void Dispose()
    {
        foreach (var gameObject in Collection) gameObject.Dispose();
        Collection.Clear();
    }
    #endregion
}
