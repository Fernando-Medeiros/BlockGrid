namespace GameUI.Core.Nodes;

public sealed class NodeTree
{
    public NodeTree(NodeGraphic nodeGraphic)
    {
        Position = (_row, _column);
        _nodes[_row].Add(nodeGraphic);

        _column++;
        if (_column >= GameEnvironment.MAX_COLUMN) { _row++; _column = 0; };

        App.Subscribe(Event.LoadResource, (_) => Mount());
    }

    #region Linked
    private static byte _row;
    private static byte _column;
    private static readonly IReadOnlyList<IList<NodeGraphic>> _nodes =
        [[], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], []];
    #endregion

    #region Property
    public (byte Row, byte Column) Position { get; }
    public NodeGraphic? TopLeft { get; private set; }
    public NodeGraphic? Top { get; private set; }
    public NodeGraphic? TopRight { get; private set; }
    public NodeGraphic? Left { get; private set; }
    public NodeGraphic? Right { get; private set; }
    public NodeGraphic? BottomLeft { get; private set; }
    public NodeGraphic? Bottom { get; private set; }
    public NodeGraphic? BottomRight { get; private set; }
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