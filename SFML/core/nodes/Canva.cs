namespace SFMLGame.core.nodes;

public sealed class Canva(INode2D node)
{
    private INode2D Node => node;

    private readonly Vector2f position = new(node.Position.X, node.Position.Y);

    public void Draw(RenderWindow window)
    {
        DrawSurface(window);
        DrawSprite(window);
    }

    #region Layer
    private void DrawSurface(RenderWindow window)
    {
        var sprite = Content.GetResource(Node.Surface);
        sprite.Color = new(255, 255, 255, Convert.ToByte(Node.Opacity));
        sprite.Position = position;
        window.Draw(sprite);
    }

    public void DrawSprite(RenderWindow window)
    {
        if (Node.Body?.Sprite is null) return;

        if (Node.Opacity is EOpacity.Regular && Node.Body is EnemyBody2D) return;

        var sprite = Content.GetResource((Sprite2D)Node.Body.Sprite);
        sprite.Color = new(255, 255, 255, Convert.ToByte(Node.Opacity));
        sprite.Position = position;
        window.Draw(sprite);
    }
    #endregion
}