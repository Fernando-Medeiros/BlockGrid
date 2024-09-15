namespace SFMLGame.core.scenes.world;

public class WorldView(FloatRect viewRect)
    : View(viewRect), IGameObject
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

                Collection[row].Add(new Node2D(position2D));
            }
        }

        foreach (var nodeList in Collection)
            foreach (var node in nodeList)
            {
                node.Position2D.Deconstruct(out var row, out var column, out _, out _);

                node.Navigation[EDirection.Left] = Collection.ElementAtOrDefault(row)?.ElementAtOrDefault(column - 1);
                node.Navigation[EDirection.Right] = Collection.ElementAtOrDefault(row)?.ElementAtOrDefault(column + 1);

                node.Navigation[EDirection.Top] = Collection.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column);
                node.Navigation[EDirection.TopLeft] = Collection.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column - 1);
                node.Navigation[EDirection.TopRight] = Collection.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column + 1);

                node.Navigation[EDirection.Bottom] = Collection.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column);
                node.Navigation[EDirection.BottomLeft] = Collection.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column - 1);
                node.Navigation[EDirection.BottomRight] = Collection.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column + 1);
            }
    }

    public virtual void LoadEvents()
    {
        Global.Subscribe(EEvent.Scene, OnSceneChanged);
        Global.Subscribe(EEvent.Region, OnRegionChanged);
        Global.Subscribe(EEvent.SaveGame, OnRegionSaved);
        Global.Subscribe(EEvent.Camera, OnCameraChanged);
        Global.Subscribe(EEvent.KeyPressed, OnZoomChanged);
        Global.Subscribe(EEvent.MouseButtonPressed, OnNodeSelected);
    }

    public void Draw(RenderWindow window)
    {
        window.SetView(this);

        foreach (var nodeList in Collection)
            foreach (var node in nodeList)
            {
                if (node.Opacity is EOpacity.Dark)
                    continue;

                var (posX, posY, width, height) = (node.Position2D.X, node.Position2D.Y, Size.X / 2, Size.Y / 2);

                if (posX < Center.X - width || posX > Center.X + width)
                    continue;
                if (posY < Center.Y - height || posY > Center.Y + height)
                    continue;

                node.Draw(window);
            }
    }
    #endregion

    #region Event
    private void OnSceneChanged(object? sender)
    {
        if (App.CurrentScene != EScene.World) return;

        Content.DeserializeFromXml("0");
    }

    // TODO :: Refinar
    private void OnRegionSaved(object? sender)
    {
        if (App.CurrentScene != EScene.World) return;

        var schema = new RegionSchema
        {
            Name = "0",
            ESurface = App.RegionSurface
        };

        foreach (var nodeList in Collection)
        {
            schema.Nodes.Add([]);

            foreach (var node in nodeList)
            {
                node.Position2D.Deconstruct(out var row, out var column, out _, out _);

                var nodeSchema = new NodeSchema()
                {
                    Row = row,
                    Column = column,
                    Opacity = node.Opacity,
                };

                if (node.Body != null)
                    nodeSchema.Body = new()
                    {
                        Type = node.Body.Type ?? default,
                        Sprite = node.Body.Sprite ?? default
                    };

                foreach (var item in node.GameItems)
                    nodeSchema.Items.Add(new() { Sprite = item.Sprite });

                schema.Nodes.ElementAt(row).Add(nodeSchema);

                node.SetBody(null);
                node.GameItems.Clear();
                node.SetOpacity(EOpacity.Dark);
            }
        }

        Content.SerializeToXml(schema, "0");
    }

    // TODO :: Refinar
    private void OnRegionChanged(object? sender)
    {
        if (App.CurrentScene != EScene.World) return;

        if (sender is RegionSchema regionSchema)
        {
            Global.Invoke(EEvent.Transport, regionSchema.ESurface);

            foreach (var nodeSchemas in regionSchema.Nodes)
                foreach (var schema in nodeSchemas)
                {
                    var node = Collection.ElementAt(schema.Row).ElementAt(schema.Column);

                    node.SetBody(null);
                    node.GameItems.Clear();
                    node.SetOpacity(schema.Opacity);

                    foreach (var itemSchema in schema.Items)
                        node.GameItems.Add(new GameItem() { Sprite = itemSchema.Sprite });

                    if (schema.Body is null) continue;

                    if (schema.Body.Type is EBody.Player)
                        App.CurrentPlayer?.Dispose();

                    Global.Invoke(EEvent.Transport, Factory.Get(schema.Body.Type, node));
                    Global.Invoke(EEvent.Camera, node.Position2D);
                }
        }

        if (App.CurrentPlayer == null)
        {
            var node = Collection.ElementAt(Global.MAX_ROW / 2).ElementAt(Global.MAX_COLUMN / 2);
            Global.Invoke(EEvent.Transport, Factory.Get(EBody.Player, node));
            Global.Invoke(EEvent.Camera, node.Position2D);
        }
    }

    private void OnNodeSelected(object? sender)
    {
        if (App.CurrentScene != EScene.World) return;

        if (sender is MouseDTO mouse)
        {
            var absolutePosition = App.MapCoords(mouse.X, mouse.Y, this);

            var posY = absolutePosition.Y - (Global.RECT / 2);
            var posX = absolutePosition.X - (Global.RECT / 2);

            int row = Math.Max(0, Math.Min(Convert.ToInt32(posY / Global.RECT), Global.MAX_ROW - 1));
            int column = Math.Max(0, Math.Min(Convert.ToInt32(posX / Global.RECT), Global.MAX_COLUMN - 1));

            Global.Invoke(EEvent.Transport, Collection.ElementAt(row).ElementAt(column));

#if DEBUG
            Collection.ElementAt(row).ElementAt(column).GameItems.Add(new GameItem());
#endif
        }
    }

    private void OnZoomChanged(object? sender)
    {
        if (App.CurrentScene != EScene.World) return;

        if (sender is Key.Z)
        {
            if (Size.X <= ViewRect.Width / 2) return;
            Zoom(0.9f);
            OnCameraChanged(null);
        }

        if (sender is Key.X)
        {
            if (Size.Y >= ViewRect.Height) return;
            Zoom(1.1f);
            OnCameraChanged(null);
        }
    }

    protected void OnCameraChanged(object? sender)
    {
        if (App.CurrentScene != EScene.World) return;

        App.CurrentPosition.Deconstruct(out var row, out var column, out var posX, out var posY);

        var (width, height) = (Size.X, Size.Y);

        float scrollX = posX - (width / 2);
        float scrollY = posY - (height / 2);

        scrollX = Math.Max(0, Math.Min(scrollX, Global.WORLD_WIDTH - width));
        scrollY = Math.Max(0, Math.Min(scrollY, Global.WORLD_HEIGHT - height));

        Center = new Vector2f(scrollX + (width / 2), scrollY + (height / 2));

#if DEBUG
        Global.Invoke(EEvent.Logger, new LoggerDTO(ELogger.Debug, $"R:{row} C:{column} X:{Center.X} Y:{Center.Y}"));
#endif
    }
    #endregion
}

// TODO :: Otimizar a renderização e o uso do processador.
// Este objeto é redundante porque renderiza o world, porem o exibe em uma escala menor.
public sealed class WorldMapView : WorldView
{
    public WorldMapView(FloatRect rect) : base(rect)
    {
        Viewport = new(0.85f, 0, 0.15f, 0.15f);
    }

    public override void LoadEvents()
    {
        Global.Subscribe(EEvent.Camera, OnCameraChanged);
    }
}