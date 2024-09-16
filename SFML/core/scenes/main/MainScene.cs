namespace SFMLGame.core.scenes.main;

public sealed class MainScene(FloatRect viewRect)
    : View(viewRect), IGameObject
{
    private IList<IGameObject> Collection { get; } = [];

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
}
