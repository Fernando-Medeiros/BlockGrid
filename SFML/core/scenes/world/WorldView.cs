namespace SFMLGame.core.scenes.world;

public sealed class WorldView(FloatRect rect) : View(rect), IGameObject
{
    private FloatRect Rect { get; } = rect;
    private IList<IList<INode2D>> Collection { get; } = [];

    #region Build
    public void LoadEvents(RenderWindow window)
    {
        window.KeyPressed += OnZoomChanged;
        window.MouseWheelScrolled += OnZoomChanged;

        Global.Subscribe(EEvent.Camera, OnCenterChanged);
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

                // Tests 
                if (row == 21 && column == 21)
                    node.SetBody(new PlayerBody2D(node));
                else if (row % 3 == 0 && column % 3 == 0)
                    node.SetBody(new EnemyBody2D(node));
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
    private void OnZoomChanged(object? sender, EventArgs e)
    {
        int option = e switch
        {
            MouseWheelScrollEventArgs x => (int)x.Delta,
            KeyEventArgs x => Enum.GetName(x.Code) == "Z" ? 1 : Enum.GetName(x.Code) == "X" ? -1 : 0,
            _ => 0
        };

        if (option == 1)
        {
            if (Size.X <= Rect.Width / 2) return;
            Zoom(0.9f);
        }

        if (option == -1)
        {
            if (Size.Y >= Rect.Height) return;
            Zoom(1.1f);
        }
    }

    private void OnCenterChanged(object? sender)
    {
        if (sender is Position2D position2D)
        {
            var (row, column, posX, posY) = position2D;

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
