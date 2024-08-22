namespace MONOGame.Core.Nodes;

public sealed class Node2D : INode2D
{
    public Node2D(Position2D position)
    {
        Position = position;
        Canva = new Canva(Position);

        App.Global.Subscribe(CoreEvent.LoadScene, Load);
    }

    #region Property
    private IBody2D? _body;
    private Surface2D _surface;

    public Canva Canva { get; }
    public Position2D Position { get; }

    public IBody2D? Body { get => _body; set { _body = value; Draw(); } }
    public Surface2D Surface { get => _surface; set { _surface = value; Draw(); } }

    private INode2D? Top { get; set; }
    private INode2D? Left { get; set; }
    private INode2D? Right { get; set; }
    private INode2D? Bottom { get; set; }
    private INode2D? TopLeft { get; set; }
    private INode2D? TopRight { get; set; }
    private INode2D? BottomLeft { get; set; }
    private INode2D? BottomRight { get; set; }
    #endregion

    #region Build
    private void Load(object? sender)
    {
        if (Is.Null(sender) || Is.Not<ScenePackage>(sender)) return;

        var (row, column, _, _) = Position;

        _surface = ((ScenePackage)sender!).Surface?.ElementAtOrDefault(row)?.ElementAtOrDefault(column) ?? default;

        Draw();
    }
    #endregion

    #region Action
    public void Draw()
    {
        Canva.Body = Body;
        Canva.Surface = Surface;
    }

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
    #endregion

    public void Mount(IReadOnlyList<IList<INode2D>> nodes)
    {
        var (row, column, _, _) = Position;

        Left ??= nodes.ElementAtOrDefault(row)?.ElementAtOrDefault(column - 1);
        Right ??= nodes.ElementAtOrDefault(row)?.ElementAtOrDefault(column + 1);

        Top ??= nodes.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column);
        TopLeft ??= nodes.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column - 1);
        TopRight ??= nodes.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column + 1);

        Bottom ??= nodes.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column);
        BottomLeft ??= nodes.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column - 1);
        BottomRight ??= nodes.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column + 1);

        // Tests 
        if (row == 21 && column == 21) nodes[row][column].Body = new PlayerBody2D(nodes[row][column]);
        else if (row % 3 == 0 && column % 3 == 0) nodes[row][column].Body = new EnemyBody2D(nodes[row][column]);
    }
}
