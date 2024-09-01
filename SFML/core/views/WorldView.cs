using System.Xml.Linq;

namespace SFMLGame.core.views;

public sealed class WorldView(FloatRect rect) : View(rect)
{
    private readonly FloatRect _viewRect = rect;
    private readonly IList<IList<INode2D>> _nodes = [];

    private const float _worldHeight = Global.MAX_ROW * Global.RECT;
    private const float _worldWidth = Global.MAX_COLUMN * Global.RECT;

    #region Build
    public void ConfigureNodes()
    {
        for (byte row = 0; row < Global.MAX_ROW; row++)
        {
            _nodes.Add([]);

            for (byte column = 0; column < Global.MAX_COLUMN; column++)
            {
                var position2D = new Position2D(row, column, column * Global.RECT, row * Global.RECT);

                _nodes[row].Add(new Node2D(position2D));
            }
        }

        foreach (var nodeList in _nodes)
            foreach (var node in nodeList)
            {
                var (row, column, _, _) = node.Position;

                node.Navigation[EDirection.Left] = _nodes.ElementAtOrDefault(row)?.ElementAtOrDefault(column - 1);
                node.Navigation[EDirection.Right] = _nodes.ElementAtOrDefault(row)?.ElementAtOrDefault(column + 1);

                node.Navigation[EDirection.Top] = _nodes.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column);
                node.Navigation[EDirection.TopLeft] = _nodes.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column - 1);
                node.Navigation[EDirection.TopRight] = _nodes.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column + 1);

                node.Navigation[EDirection.Bottom] = _nodes.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column);
                node.Navigation[EDirection.BottomLeft] = _nodes.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column - 1);
                node.Navigation[EDirection.BottomRight] = _nodes.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column + 1);

                // Tests 
                if (row == 21 && column == 21)
                    node.SetBody(new PlayerBody2D(node));
                else if (row % 3 == 0 && column % 3 == 0)
                    node.SetBody(new EnemyBody2D(node));
            }
    }

    public void ConfigureListeners(RenderWindow window)
    {
        window.MouseWheelScrolled += OnZoomChanged;

        Global.Subscribe(EEvent.Camera, OnCenterChanged);
    }

    public void Draw(RenderWindow window)
    {
        foreach (var nodeList in _nodes)
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
    private void OnZoomChanged(object? sender, MouseWheelScrollEventArgs e)
    {
        if (e.Delta == 1)
        {
            if (Size.X <= _viewRect.Width / 2) return;
            Zoom(0.9f);

            Global.Invoke(EEvent.Logger, new Logger(ELogger.General, $"Zoom changed :: +1.."));
        }

        if (e.Delta == -1)
        {
            if (Size.Y >= _viewRect.Height) return;
            Zoom(1.1f);

            Global.Invoke(EEvent.Logger, new Logger(ELogger.General, $"Zoom changed :: -1.."));
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

            scrollX = Math.Max(0, Math.Min(scrollX, _worldWidth - width));
            scrollY = Math.Max(0, Math.Min(scrollY, _worldHeight - height));

            Center = new Vector2f(scrollX + (width / 2), scrollY + (height / 2));

            Global.Invoke(EEvent.Logger, new Logger(ELogger.Debug, $"R:{row} C:{column} X:{Center.X} Y:{Center.Y}"));
        }
    }
    #endregion
}
