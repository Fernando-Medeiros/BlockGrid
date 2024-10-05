namespace SFMLGame.core.scenes.main;

public sealed class MainScene : View, IGameObject
{
    private FloatRect ViewRect { get; init; }
    private IList<IGameObject> Collection { get; } = [];

    public MainScene(FloatRect viewRect) : base(viewRect)
    {
        ViewRect = viewRect;

        Global.Subscribe(EEvent.Scene, OnSceneChanged);
    }

    #region Initialize
    private void OnSceneChanged(object? sender)
    {
        if (sender is EScene.Main)
        {
            LoadContent();
            LoadEvents();
            return;
        }

        Dispose();
    }
    #endregion

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
        GC.Collect(GC.GetGeneration(Collection), GCCollectionMode.Forced);
    }
    #endregion
}
