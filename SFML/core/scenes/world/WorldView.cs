namespace SFMLGame.core.scenes.world;

public class WorldView(FloatRect viewRect)
    : View(viewRect), IGameObject, IDisposable
{
    private FloatRect ViewRect { get; } = viewRect;
    private static IList<IList<INode2D>> Collection { get; } = [];

    #region Build 
    public void LoadContent()
    {
        for (byte row = 0; row < Global.MAX_ROW; row++)
        {
            Collection.Add([]);

            for (byte column = 0; column < Global.MAX_COLUMN; column++)
            {
                var position2D = new Position2D(row, column, column * Global.RECT, row * Global.RECT);

                Collection.ElementAt(row).Add(new Node2D(position2D));
            }
        }
    }

    public virtual void LoadEvents()
    {
        Node2D.Navigation += OnNodeNavigation;

        Global.Subscribe(EEvent.Region, OnRegionChanged);
        Global.Subscribe(EEvent.SaveGame, OnRegionSaved);
        Global.Subscribe(EEvent.Camera, OnCameraChanged);
        Global.Subscribe(EEvent.KeyPressed, OnZoomChanged);
        Global.Subscribe(EEvent.MouseButtonPressed, OnNodeSelected);
    }

    public void Draw(RenderWindow window)
    {
        window.SetView(this);

        float width = (Size.X / 2) + Global.RECT;
        float height = (Size.Y / 2) + Global.RECT;
        float minX = Center.X - width;
        float maxX = Center.X + width;
        float minY = Center.Y - height;
        float maxY = Center.Y + height;

        foreach (var nodes in Collection)
            foreach (var node in nodes)
            {
                var (posX, posY) = node.Position2D.TopLeft;

                if (posX < minX || posX > maxX || posY < minY || posY > maxY)
                    continue;

                node.Draw(window);
            }
    }
    #endregion

    #region Event
    private INode2D? OnNodeNavigation(EDirection direction, Position2D position2D)
    {
        var (row, column) = position2D.Matrix;

        return direction switch
        {
            EDirection.Left => Collection.ElementAtOrDefault(row)?.ElementAtOrDefault(column - 1),
            EDirection.Right => Collection.ElementAtOrDefault(row)?.ElementAtOrDefault(column + 1),
            EDirection.Top => Collection.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column),
            EDirection.TopLeft => Collection.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column - 1),
            EDirection.TopRight => Collection.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column + 1),
            EDirection.Bottom => Collection.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column),
            EDirection.BottomLeft => Collection.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column - 1),
            EDirection.BottomRight => Collection.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column + 1),
            _ => null
        };
    }

    // TODO :: Refatorar
    private void OnRegionSaved(object? sender)
    {
        var regionSchema = new RegionSchema
        {
            Name = "0",
            Biome = App.CurrentBiome,
        };

        foreach (var nodes in Collection)
            foreach (var node in nodes)
            {
                if (node.Body is null && node.Objects.Count == 0) continue;

                var (row, column) = node.Position2D.Matrix;

                var nodeSchema = new NodeSchema()
                {
                    Row = row,
                    Column = column,
                };

                if (node.Body is IBody2D body)
                    nodeSchema.Body = new()
                    {
                        Type = (EBody)body.Type,
                        Sprite = (ESprite)body.Sprite
                    };

                foreach (var item in node.Objects)
                    nodeSchema.Objects.Add(new() { Sprite = item.Sprite });

                regionSchema.Nodes.Add(nodeSchema);

                node.Clear();
            }

        Content.SerializeSchema(regionSchema);
    }

    // TODO :: Refatorar
    private void OnRegionChanged(object? sender)
    {
        if (sender is RegionSchema regionSchema)
        {
            Global.Invoke(EEvent.Transport, regionSchema.Biome);

            foreach (var schema in regionSchema.Nodes)
            {
                var node = Collection.ElementAt(schema.Row).ElementAt(schema.Column);

                node.Clear();

                foreach (var itemSchema in schema.Objects)
                    node.Objects.Add(new Object2D() { Sprite = itemSchema.Sprite });

                if (schema.Body is null) continue;

                if (schema.Body.Type is EBody.Player)
                {
                    App.CurrentPlayer?.Dispose();
                    Global.Invoke(EEvent.Transport, Factory.Build(schema.Body.Type, node));
                    Global.Invoke(EEvent.Camera, node.Position2D);
                    continue;
                }

                Factory.Build(schema.Body.Type, node);
            };
        }

        if (App.CurrentPlayer == null)
        {
            var node = Collection.ElementAt(Global.MAX_ROW / 2).ElementAt(Global.MAX_COLUMN / 2);
            Global.Invoke(EEvent.Transport, Factory.Build(EBody.Player, node));
            Global.Invoke(EEvent.Camera, node.Position2D);
        }
    }

    private void OnNodeSelected(object? sender)
    {
        if (Collection.Count <= 0) return;

        if (sender is MouseDTO mouse)
        {
            var absolutePosition = App.MapCoords(mouse.X, mouse.Y, this);

            var posY = absolutePosition.Y - (Global.RECT / 2);
            var posX = absolutePosition.X - (Global.RECT / 2);

            int row = Math.Max(0, Math.Min(Convert.ToInt32(posY / Global.RECT), Global.MAX_ROW - 1));
            int column = Math.Max(0, Math.Min(Convert.ToInt32(posX / Global.RECT), Global.MAX_COLUMN - 1));

            INode2D node = Collection.ElementAt(row).ElementAt(column);

            if (node.Opacity is EOpacity.Light)
                Global.Invoke(EEvent.Transport, node);
        }
    }

    private void OnZoomChanged(object? sender)
    {
        if (sender is Key.Z)
        {
            if (Size.X <= ViewRect.Width / 2) return;
            Zoom(0.95f);
            OnCameraChanged(null);
        }

        if (sender is Key.X)
        {
            if (Size.Y >= ViewRect.Height) return;
            Zoom(1.05f);
            OnCameraChanged(null);
        }
    }

    protected void OnCameraChanged(object? sender)
    {
        var (X, Y) = App.CurrentPosition.TopLeft;

        var (width, height) = (Size.X, Size.Y);

        float scrollX = X - (width / 2);
        float scrollY = Y - (height / 2);

        scrollX = Math.Max(0, Math.Min(scrollX, Global.WORLD_WIDTH - width));
        scrollY = Math.Max(0, Math.Min(scrollY, Global.WORLD_HEIGHT - height));

        Center = new Vector2f(scrollX + (width / 2), scrollY + (height / 2));
    }
    #endregion

    #region Dispose
    public new void Dispose()
    {
        Node2D.Navigation -= OnNodeNavigation;

        Global.UnSubscribe(EEvent.Region, OnRegionChanged);
        Global.UnSubscribe(EEvent.SaveGame, OnRegionSaved);
        Global.UnSubscribe(EEvent.Camera, OnCameraChanged);
        Global.UnSubscribe(EEvent.KeyPressed, OnZoomChanged);
        Global.UnSubscribe(EEvent.MouseButtonPressed, OnNodeSelected);

        foreach (var nodes in Collection)
            foreach (var node in nodes)
                node.Dispose();

        Collection.Clear();
        GC.Collect(GC.GetGeneration(Collection), GCCollectionMode.Aggressive);
    }
    #endregion
}
