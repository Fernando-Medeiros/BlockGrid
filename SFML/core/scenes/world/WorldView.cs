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

    // TODO :: Refatorar para salvar apenas os dados dos nodes ocupados na região
    private void OnSaveRegionChanged(object? sender)
    {
        var region = App.Region;
        region.Nodes.Clear();
        region.UpdatedOn = DateTime.Now;

        foreach (var nodes in Collection)
            foreach (var node in nodes)
            {
                if (node.Body is PlayerBody2D)
                {
                    node.Body.Dispose();
                    node.SetBody(null);
                }

                if (node.Body is null && node.Objects.Count == 0) continue;

                region.Nodes.Add(new()
                {
                    Position = node.Position2D.Matrix,
                });

                node.Dispose();
            }

        FileHandler.SerializeSchema(EFolder.Regions, region, region.Token);
    }

    // TODO :: Refatorar para carregar somente os nodes da região e encontrar uma forma de desacoplar o player do node2d
    private void OnLoadRegionChanged(object? sender)
    {
        if (sender is RegionSchema regionSchema)
        {
            foreach (var schema in regionSchema.Nodes)
            {
                var (_row, _column) = schema.Position;

                var node = Collection
                   .ElementAt(_row)
                   .ElementAt(_column);

                node.Dispose();
            }

            var (row, column) = App.Player.RegionPosition;

            var _node = Collection
                .ElementAt(row)
                .ElementAt(column);

            Factory.Build(EBody.Player, _node);
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
