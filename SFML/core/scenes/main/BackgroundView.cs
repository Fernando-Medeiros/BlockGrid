namespace SFMLGame.core.scenes.main;

public sealed class BackgroundView : IGameObject
{
    private IList<IList<(ESprite, Position2D)>> Collection { get; } = [];

    #region Build
    public void LoadContent()
    {
        int maxRow = Global.WINDOW_HEIGHT / Global.RECT + 1;
        int maxColumn = Global.WINDOW_WIDTH / Global.RECT + 1;

        for (byte row = 0; row < maxRow; row++)
        {
            Collection.Add([]);
            for (byte column = 0; column < maxColumn; column++)
            {
                var sprite = Factory.Shuffle(ESurface.Soil);
                var position2D = new Position2D(row, column, column * Global.RECT, row * Global.RECT);

                Collection.ElementAt(row).Add((sprite, position2D));
            }
        }
    }

    public void LoadEvents()
    {
    }

    public void Draw(RenderWindow window)
    {
        foreach (var nodeList in Collection)
            foreach (var (resource, position) in nodeList)
            {
                var sprite = Content.GetResource(resource);
                sprite.Position = position;
                window.Draw(sprite);
            }
    }
    #endregion
}
