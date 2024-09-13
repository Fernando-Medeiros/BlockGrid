namespace SFMLGame.core.nodes;

public sealed class Node2D : INode2D
{
    public Node2D(Position2D position2D)
    {
        Position2D = position2D;
        Position = new(position2D.X, position2D.Y);
    }

    #region Property
    private Vector2f Position { get; }

    public Position2D Position2D { get; }
    public EOpacity Opacity { get; private set; }
    public IBody2D? Body { get; private set; }
    public ESurface Surface { get; private set; }
    public Dictionary<EDirection, INode2D?> Navigation { get; } = [];
    #endregion

    #region Action
    public void SetBody(IBody2D? body) => Body = body;

    public void SetSurface(ESurface? surface) => Surface = surface ?? default;

    public void SetOpacity(EOpacity opacity) => Opacity = opacity;

    public INode2D? Get(EDirection direction) => Navigation[direction];

    public INode2D? Get(params EDirection[] directions)
    {
        INode2D? node = this;
        foreach (var direction in directions)
            node = node?.Get(direction);
        return node;
    }

    public void Draw(RenderWindow window)
    {
        DrawSurface(window);
        DrawSprite(window);
    }
    #endregion

    #region Canva Layers
    private void DrawSurface(RenderWindow window)
    {
        var sprite = Content.GetResource(Surface);
        sprite.Color = new(255, 255, 255, Convert.ToByte(Opacity));
        sprite.Position = Position;
        window.Draw(sprite);
    }

    private void DrawSprite(RenderWindow window)
    {
        if (Body?.Sprite is null) return;
        if (Opacity is EOpacity.Regular && Body is EnemyBody2D) return;

        var sprite = Content.GetResource((ESprite)Body.Sprite);
        sprite.Color = new(255, 255, 255, Convert.ToByte(Opacity));
        sprite.Position = Position;
        window.Draw(sprite);
    }
    #endregion
}
