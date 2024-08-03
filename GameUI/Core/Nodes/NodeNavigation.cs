namespace GameUI.Core.Nodes;

public sealed class NodeNavigation
{
    public NodeNavigation(INode node)
    {
        Position = (_row, _column);
        _nodes[_row].Add(node);

        _column++;
        if (_column >= GameEnvironment.MAX_COLUMN) { _row++; _column = 0; };

        App.Subscribe(Event.LoadResource, (_) => Mount());
    }

    #region Linked
    private static byte _row;
    private static byte _column;
    private static readonly IReadOnlyList<IList<INode>> _nodes =
        [[], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], []];
    #endregion

    #region Property
    public (byte Row, byte Column) Position { get; }
    public INode? TopLeft { get; private set; }
    public INode? Top { get; private set; }
    public INode? TopRight { get; private set; }
    public INode? Left { get; private set; }
    public INode? Right { get; private set; }
    public INode? BottomLeft { get; private set; }
    public INode? Bottom { get; private set; }
    public INode? BottomRight { get; private set; }
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