using SFMLGame.core.scenes.world;

namespace SFMLGame.core.scenes;

public sealed class WorldScene : View, IView
{
    private IView? World { get; set; }
    private FloatRect ViewRect { get; init; }
    private IList<IView> Collection { get; } = [];

    public WorldScene(FloatRect viewRect) : base(viewRect)
    {
        ViewRect = viewRect;

        Global.Subscribe(EEvent.SceneChanged, OnSceneChanged);
    }

    #region Initialize
    private void OnSceneChanged(object? sender)
    {
        if (sender is EScene.World)
        {
            Build();
            Event();
            return;
        }

        Dispose();
    }
    #endregion

    #region Build
    public void Build()
    {
        World = new WorldView(ViewRect);
        World?.Build();

        Collection.Add(new PlayerHUD());
        Collection.Add(new EnemyHUD());
        Collection.Add(new LoggerHUD());
        Collection.Add(new CommandHUD());
        Collection.Add(new WorldMapHUD());
        Collection.Add(new DeveloperHUD());

        foreach (var view in Collection) view.Build();
    }

    public void Event()
    {
        World?.Event();

        foreach (var view in Collection) view.Event();
    }

    public void Render(RenderWindow window)
    {
        World?.Render(window);

        window.SetView(this);

        foreach (var view in Collection) view.Render(window);
    }
    #endregion

    #region Dispose
    public new void Dispose()
    {
        foreach (var view in Collection) view.Dispose();

        Collection.Clear();

        World?.Dispose();
        World = null;

        GC.Collect(GC.GetGeneration(Collection), GCCollectionMode.Aggressive);
    }
    #endregion
}
