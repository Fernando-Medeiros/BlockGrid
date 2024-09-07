namespace SFMLGame.core.scenes.world;

public sealed class WorldView : View, IGameObject
{
    private FloatRect Rect { get; set; }
    private static Position2D? Position2D { get; set; }
    private static IList<IList<INode2D>> Collection { get; } = [];

    public WorldView(FloatRect rect) : base(rect) => Rect = rect;

    #region Build
    public void LoadEvents(RenderWindow window)
    {
        Global.Subscribe(EEvent.Scene, OnSceneChanged);
        Global.Subscribe(EEvent.Camera, OnCenterChanged);
        Global.Subscribe(EEvent.KeyPressed, OnZoomChanged);
    }

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
                var (row, column, _, _) = node.Position;

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

    public void Draw(RenderWindow window)
    {
        window.SetView(this);

        foreach (var nodeList in Collection)
            foreach (var node in nodeList)
            {
                var (posX, posY, width, height) = (node.Position.X, node.Position.Y, Size.X / 2, Size.Y / 2);

                if (posX < Center.X - width || posX > Center.X + width)
                    continue;
                if (posY < Center.Y - height || posY > Center.Y + height)
                    continue;

                node.Canva.Draw(window);
            }
    }
    #endregion

    #region Event
    private void OnSceneChanged(object? sender)
    {
        // TODO :: Corrigir o objeto duplicado do PlayerBody2D sempre que a cena mudar;
        // TODO :: Trabalhar em alguma estrutura pra manter e carregar o estado do player e do mapa;
        if (sender is EScene.World)
        {
            Content.LoadScene();

            var node = Collection.ElementAt(21).ElementAt(21);
            node.SetBody(new PlayerBody2D(node));
        }
    }

    private void OnZoomChanged(object? sender)
    {
        if (sender is Key.Z)
        {
            if (Size.X <= Rect.Width / 2) return;
            Zoom(0.9f);
            Global.Invoke(EEvent.Camera, Position2D);
        }

        if (sender is Key.X)
        {
            if (Size.Y >= Rect.Height) return;
            Zoom(1.1f);
            Global.Invoke(EEvent.Camera, Position2D);
        }
    }

    private void OnCenterChanged(object? sender)
    {
        if (sender is Position2D position2D) Position2D = position2D;

        if (Position2D != null)
        {
            var (row, column, posX, posY) = Position2D;

            var (width, height) = (Size.X, Size.Y);

            float scrollX = posX - (width / 2);
            float scrollY = posY - (height / 2);

            scrollX = Math.Max(0, Math.Min(scrollX, Global.WORLD_WIDTH - width));
            scrollY = Math.Max(0, Math.Min(scrollY, Global.WORLD_HEIGHT - height));

            Center = new Vector2f(scrollX + (width / 2), scrollY + (height / 2));

            Global.Invoke(EEvent.Logger, new Logger(ELogger.Debug, $"R:{row} C:{column} X:{Center.X} Y:{Center.Y}"));
        }
    }
    #endregion
}
