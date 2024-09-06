namespace SFMLGame.core.scenes.world;

public sealed class WorldUIView(FloatRect rect) : View(rect), IGameObject
{
    private IList<IGameObject> Collection { get; } = [];

    #region Build
    public void LoadEvents(RenderWindow window)
    {
        foreach (var gameObject in Collection) gameObject.LoadEvents(window);
    }

    public void LoadContent()
    {
        Collection.Add(new PlayerHUD());
        Collection.Add(new EnemyHUD());
        Collection.Add(new LoggerHUD());

        foreach (var gameObject in Collection) gameObject.LoadContent();
    }

    public void Draw(RenderWindow window)
    {
        window.SetView(this);

        foreach (var gameObject in Collection) gameObject.Draw(window);
    }
    #endregion
}
