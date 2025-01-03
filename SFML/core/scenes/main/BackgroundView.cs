﻿namespace SFMLGame.core.scenes.main;

public sealed class BackgroundView : IView, IDisposable
{
    private (byte Row, byte Column) Center { get; set; } = (0, 0);
    private IList<IList<(ETerrain, Position2D)>> Collection { get; } = [];
    private int MaxRow { get; } = (App.Configuration.WindowResolution.Height / Global.RECT) + 1;
    private int MaxColumn { get; } = (App.Configuration.WindowResolution.Width / Global.RECT) + 1;

    #region Build
    public void Build()
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

    public void Event()
    {
        Global.Subscribe(EEvent.MouseMoved, OnMouseMoved);
    }

    private float Speed { get; set; }
    private float Radius { get; set; } = 7;

    public void Render(RenderWindow window)
    {
        foreach (var nodeList in Collection)
            foreach (var (resource, position) in nodeList)
            {
                Color opacity = Factory.Color(EOpacity.Opaque);

                int dx = position.Column - Center.Column;
                int dy = position.Row - Center.Row;
                double distance = Math.Sqrt((dx * dx) + (dy * dy));

                if (distance < Radius + 4)
                    opacity = Factory.Color(EOpacity.Regular);

                if (distance < Radius)
                    opacity = Factory.Color(EOpacity.Light);

                var sprite = Content.GetResource<Sprite>(resource);
                sprite.Color = opacity;
                sprite.Position = position;
                window.Draw(sprite);
            }

        Radius += Speed;
        Speed += Radius >= 11 ? -0.01f : 0.01f;
    }
    #endregion

    #region Event
    private void OnMouseMoved(object? sender)
    {
        if (sender is MouseDTO mouse)
        {
            var posY = mouse.Y - (Global.RECT / 2);
            var posX = mouse.X - (Global.RECT / 2);

            int row = Math.Max(0, Math.Min(Convert.ToInt32(posY / Global.RECT), MaxRow - 1));
            int column = Math.Max(0, Math.Min(Convert.ToInt32(posX / Global.RECT), MaxColumn - 1));

            Center = ((byte)row, (byte)column);
        }
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
        Global.Unsubscribe(EEvent.MouseMoved, OnMouseMoved);

        Center = (0, 0);
        Collection.Clear();

        GC.Collect(GC.GetGeneration(Collection), GCCollectionMode.Forced);
    }
    #endregion
}
