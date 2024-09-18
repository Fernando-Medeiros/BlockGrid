namespace SFMLGame.core.scenes.main;

public sealed class BackgroundView : IGameObject
{
    private (byte Row, byte Column) Center { get; set; } = (0, 0);
    private IList<IList<(ESprite, Position2D)>> Collection { get; } = [];
    private int MaxRow { get; } = Global.WINDOW_HEIGHT / Global.RECT + 1;
    private int MaxColumn { get; } = Global.WINDOW_WIDTH / Global.RECT + 1;

    #region Build
    public void LoadContent()
    {
        for (byte row = 0; row < MaxRow; row++)
        {
            Collection.Add([]);

            for (byte column = 0; column < MaxColumn; column++)
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
        Global.Subscribe(EEvent.MouseMoved, OnMouseMoved);
    }

    public void Draw(RenderWindow window)
    {
        int radius = 13;

        foreach (var nodeList in Collection)
            foreach (var (resource, position) in nodeList)
            {
                Color opacity = Factory.Color(EOpacity.Opaque);

                int dx = position.Column - Center.Column;
                int dy = position.Row - Center.Row;
                double distance = Math.Sqrt(dx * dx + dy * dy);

                if (distance <= radius)
                    opacity = Factory.Color(EOpacity.Light);

                var sprite = Content.GetResource(resource);
                sprite.Color = opacity;
                sprite.Position = position;
                window.Draw(sprite);
            }
    }
    #endregion

    #region Event
    private void OnMouseMoved(object? sender)
    {
        if (App.CurrentScene != EScene.Main) return;

        if (sender is MouseDTO mouse)
        {
            var absolutePosition = App.MapCoords(mouse.X, mouse.Y, EScene.Main);

            var posY = absolutePosition.Y - (Global.RECT / 2);
            var posX = absolutePosition.X - (Global.RECT / 2);

            int row = Math.Max(0, Math.Min(Convert.ToInt32(posY / Global.RECT), MaxRow - 1));
            int column = Math.Max(0, Math.Min(Convert.ToInt32(posX / Global.RECT), MaxColumn - 1));

            Center = ((byte)row, (byte)column);
        }
    }
    #endregion
}
