namespace SFMLGame.core.scenes.world;

public sealed class WorldScene(FloatRect viewRect)
    : View(viewRect), IGameObject
{
    private IGameObject? World { get; set; }
    private FloatRect ViewRect { get; } = viewRect;
    private IList<IGameObject> Collection { get; } = [];

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
}
