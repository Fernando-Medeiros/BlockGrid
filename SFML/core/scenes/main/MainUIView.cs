namespace SFMLGame.core.scenes.main;

public sealed class MainUIView(FloatRect rect) : View(rect), IGameObject
{
    private IList<IGameObject> Collection { get; } = [];

    #region Build
    public void LoadEvents()
    {
        foreach (var gameObject in Collection) gameObject.LoadEvents();
    }

    public void LoadContent()
    {
        Collection.Add(new CommandHUD());

        foreach (var gameObject in Collection) gameObject.LoadContent();
    }

    public void Draw(RenderWindow window)
    {
        window.SetView(this);

        foreach (var gameObject in Collection) gameObject.Draw(window);
    }
    #endregion
}
