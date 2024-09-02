namespace SFMLGame.core.views;

public sealed class WorldUIView(FloatRect rect) : View(rect)
{
    private IList<IBoxShape> Collection { get; } = [];

    #region Build
    public void Add(IBoxShape boxShape)
    {
        Collection.Add(boxShape);
    }

    public void ConfigureListeners(RenderWindow window)
    {
        foreach (var boxShape in Collection) boxShape.ConfigureListeners(window);
    }

    public void Draw(RenderWindow window)
    {
        window.SetView(this);

        foreach (var boxShape in Collection) boxShape.Draw(window);
    }
    #endregion
}
