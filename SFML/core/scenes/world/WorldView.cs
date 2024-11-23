namespace SFMLGame.core.scenes.world;

public sealed class WorldView(FloatRect viewRect) : View(viewRect), IView, IDisposable
{
    private FloatRect ViewRect { get; } = viewRect;
    private static IList<IList<INode2D>> Collection { get; } = [];

    #region Build 
    public void Build()
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

    public void Event()
    {
        Node2D.Navigation += OnNodeNavigation;

        Global.Subscribe(EEvent.SchemaChanged, OnLoadRegionChanged);
        Global.Subscribe(EEvent.SaveGameChanged, OnSaveRegionChanged);
        Global.Subscribe(EEvent.CameraChanged, OnCameraChanged);
        Global.Subscribe(EEvent.KeyPressed, OnZoomChanged);
        Global.Subscribe(EEvent.MouseButtonPressed, OnNodeSelected);
    }

    public void Render(RenderWindow window)
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

    private void OnSaveRegionChanged(object? sender)
    {
        var currentRegionSchema = App.Region;
        currentRegionSchema.Nodes.Clear();
        currentRegionSchema.UpdatedOn = DateTime.Now;

        foreach (var nodes in Collection)
            foreach (var currentNode in nodes)
            {
                NodeSchema nodeSchema = new()
                {
                    Position = currentNode.Position2D.Matrix,
                };

                if (currentNode.Body2D is PlayerBody2D)
                {
                    var currentPlayerSchema = App.Player;
                    currentPlayerSchema.UpdatedOn = DateTime.Now;
                    currentPlayerSchema.RegionToken = currentRegionSchema.Token;
                    currentPlayerSchema.RegionPosition = nodeSchema.Position;

                    FileHandler.SerializeSchema(EFolder.Characters, currentPlayerSchema);
                }
                else if (currentNode.Body2D is not null)
                    nodeSchema.Body2D = new Body2DSchema()
                    {
                        Source = (EBody)currentNode.Body2D.Source,
                        Image = currentNode.Body2D.Image?.GetHashCode() ?? 0,
                    };

                foreach (var item in currentNode.Items2D)
                    nodeSchema.Items.Add(new Item2DSchema()
                    {
                        Image = item.Image?.GetHashCode() ?? 0,
                    });

                if (nodeSchema.Items.Count > 0 || nodeSchema.Body2D != null)
                    currentRegionSchema.Nodes.Add(nodeSchema);

                currentNode.Dispose();
            }

        FileHandler.SerializeSchema(EFolder.Regions, currentRegionSchema);
    }

    private void OnLoadRegionChanged(object? sender)
    {
        if (sender is RegionSchema regionSchema)
        {
            foreach (var nodes in Collection)
                foreach (var node in nodes)
                    node.Dispose();

            foreach (NodeSchema nodeSchema in regionSchema.Nodes)
            {
                var (row, column) = nodeSchema.Position;

                var currentNode = Collection
                   .ElementAt(row)
                   .ElementAt(column);

                if (nodeSchema.Body2D is Body2DSchema body2DSchema)
                    Factory.Build(body2DSchema.Source, currentNode);

                foreach (Item2DSchema itemSchema in nodeSchema.Items)
                    currentNode.Items2D.Add(new Item2D()
                    {
                        Image = (ESprite)itemSchema.Image
                    });
            }

            var (currentRow, currentColumn) = App.Player.RegionPosition;

            var playerNode = Collection
                .ElementAt(currentRow)
                .ElementAt(currentColumn);

            Factory.Build(EBody.Player, playerNode);

            Global.Invoke(EEvent.CameraChanged, playerNode.Position2D);
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
                Global.Invoke(EEvent.NodeChanged, node);
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

    private void OnCameraChanged(object? sender)
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

        Global.Unsubscribe(EEvent.SchemaChanged, OnLoadRegionChanged);
        Global.Unsubscribe(EEvent.SaveGameChanged, OnSaveRegionChanged);
        Global.Unsubscribe(EEvent.CameraChanged, OnCameraChanged);
        Global.Unsubscribe(EEvent.KeyPressed, OnZoomChanged);
        Global.Unsubscribe(EEvent.MouseButtonPressed, OnNodeSelected);

        foreach (var nodes in Collection)
            foreach (var node in nodes)
                node.Dispose();

        Collection.Clear();
        GC.Collect(GC.GetGeneration(Collection), GCCollectionMode.Aggressive);
    }
    #endregion
}
