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
                var sprite = column switch
                {
                    < 5 => Factory.Shuffle(EBiome.BorealForest),
                    < 10 => Factory.Shuffle(EBiome.Desert),
                    < 15 => Factory.Shuffle(EBiome.Savanna),
                    < 20 => Factory.Shuffle(EBiome.Tundra),
                    < 25 => Factory.Shuffle(EBiome.Forest),
                    < 30 => Factory.Shuffle(EBiome.Swamp),
                    < 35 => Factory.Shuffle(EBiome.GrassLand),
                    < 40 => Factory.Shuffle(EBiome.Snow),
                    < 45 => Factory.Shuffle(EBiome.TropicalForest),
                    < 50 => Factory.Shuffle(EBiome.Mountain),
                    < 55 => Factory.Shuffle(EBiome.Highland),
                    < 60 => Factory.Shuffle(EBiome.DarkForest),
                    _ => Factory.Shuffle(EBiome.Forest),
                };

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
                sprite.Color = Factory.Color(EOpacity.Light);
                sprite.Position = position;
                window.Draw(sprite);
            }
    }
    #endregion
}
