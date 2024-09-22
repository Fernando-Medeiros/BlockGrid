namespace SFMLGame.core.scenes.world;

public sealed class WorldScene : View, IGameObject, IDisposable
{
    private IGameObject? World { get; set; }
    private FloatRect ViewRect { get; init; }
    private IList<IGameObject> Collection { get; } = [];

    public WorldScene(FloatRect viewRect) : base(viewRect)
    {
        ViewRect = viewRect;

        Global.Subscribe(EEvent.Scene, (sender) =>
        {
            if (sender is EScene.World)
            {
                LoadContent();
                LoadEvents();
                Content.DeserializeSchema("0");
                return;
            }

            Dispose();
        });
    }

    #region Build
    public void LoadContent()
    {
        World = new WorldView(ViewRect);
        World?.LoadContent();

        Collection.Add(new PlayerHUD());
        Collection.Add(new EnemyHUD());
        Collection.Add(new LoggerHUD());
        Collection.Add(new CommandHUD());
        Collection.Add(new WorldMapHUD());
        Collection.Add(new DeveloperHUD());

        foreach (var gameObject in Collection) gameObject.LoadContent();
    }

    public void LoadEvents()
    {
        World?.LoadEvents();

        foreach (var gameObject in Collection) gameObject.LoadEvents();
    }

    public void Draw(RenderWindow window)
    {
        World?.Draw(window);

        window.SetView(this);

        foreach (var gameObject in Collection) gameObject.Draw(window);
    }
    #endregion

    #region Dispose
    public new void Dispose()
    {
        foreach (var gameObject in Collection) gameObject.Dispose();
        Collection.Clear();
        World?.Dispose();
        World = null;
    }
    #endregion
}
