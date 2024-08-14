namespace GameUI.Core.Nodes;

public sealed class NodeNavigation
{
    static NodeNavigation()
    {
        App.Subscribe(Event.LoadResource, (_) =>
        {
            for (byte row = 0; row < GameEnvironment.MAX_ROW; row++)
            {
                for (byte column = 0; column < GameEnvironment.MAX_COLUMN; column++)
                {
                    _nodes[row][column].Navigate.Mount();

                    // TESTs
                    if (row == 11 && column == 21) _nodes[row][column].Body = new PlayerBody2D(_nodes[row][column]);
                    else if (row % 3 == 0 && column % 3 == 0) _nodes[row][column].Body = new EnemyBody2D(_nodes[row][column]);
                }
            }
        });
    }

    public NodeNavigation(INode2D node)
    {
        Position = (_row, _column);
        _nodes[_row].Add(node);

        _column++;
        if (_column >= GameEnvironment.MAX_COLUMN) { _row++; _column = 0; };
    }

    #region Linked
    private static byte _row;
    private static byte _column;
    private static readonly IReadOnlyList<IList<INode2D>> _nodes =
        [[], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], []];
    #endregion

    #region Property
    public (byte Row, byte Column) Position { get; }
    public INode2D? TopLeft { get; private set; }
    public INode2D? Top { get; private set; }
    public INode2D? TopRight { get; private set; }
    public INode2D? Left { get; private set; }
    public INode2D? Right { get; private set; }
    public INode2D? BottomLeft { get; private set; }
    public INode2D? Bottom { get; private set; }
    public INode2D? BottomRight { get; private set; }
    #endregion

    #region Common
    public INode2D? GetNode(object? key)
    {
        return key switch
        {
            Key.W or Key.Up => Top,
            Key.A or Key.Left => Left,
            Key.D or Key.Right => Right,
            Key.S or Key.Down => Bottom,
            _ => null
        };
    }

    public void VisibilityTo(double opacity)
    {
        Top?.FadeTo(opacity);
        Left?.FadeTo(opacity);
        Right?.FadeTo(opacity);
        Bottom?.FadeTo(opacity);
        TopLeft?.FadeTo(opacity);
        TopRight?.FadeTo(opacity);
        BottomLeft?.FadeTo(opacity);
        BottomRight?.FadeTo(opacity);
    }
    #endregion

    private void Mount()
    {
        var (row, column) = Position;

        Left = _nodes.ElementAtOrDefault(row)?.ElementAtOrDefault(column - 1);
        Right = _nodes.ElementAtOrDefault(row)?.ElementAtOrDefault(column + 1);

        Top = _nodes.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column);
        TopLeft = _nodes.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column - 1);
        TopRight = _nodes.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column + 1);

        Bottom = _nodes.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column);
        BottomLeft = _nodes.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column - 1);
        BottomRight = _nodes.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column + 1);
    }
}