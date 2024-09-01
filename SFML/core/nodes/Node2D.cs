namespace SFMLGame.core.nodes;

public sealed class Node2D : INode2D
{
    public Node2D(Position2D position)
    {
        Position = position;
        Canva = new Canva(this);

        Global.Subscribe(EEvent.LoadScene, (sender) =>
        {
            if (sender is ScenePackage package)
                Surface = package.Surface[Position.Row][Position.Column];
        });
    }

    #region Property
    public Canva Canva { get; }
    public Position2D Position { get; }
    public EOpacity Opacity { get; private set; }
    public IBody2D? Body { get; private set; }
    public Surface2D Surface { get; private set; }
    public Dictionary<EDirection, INode2D?> Navigation { get; } = [];
    #endregion

    #region Action
    public void SetBody(IBody2D? body) => Body = body;
    public void SetOpacity(EOpacity opacity) => Opacity = opacity;

    public INode2D? Get(EDirection direction) => Navigation[direction];
    public INode2D? Get(object? keyCode) => keyCode switch
    {
        Key.W or Key.Up => Navigation[EDirection.Top],
        Key.A or Key.Left => Navigation[EDirection.Left],
        Key.D or Key.Right => Navigation[EDirection.Right],
        Key.S or Key.Down => Navigation[EDirection.Bottom],
        _ => null
    };
    #endregion
}
