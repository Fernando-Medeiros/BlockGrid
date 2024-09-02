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

    public INode2D? Get(params EDirection[] directions)
    {
        INode2D? node = this;
        foreach (var direction in directions)
            node = node?.Get(direction);
        return node;
    }
    #endregion
}
