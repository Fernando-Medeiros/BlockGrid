﻿namespace SFMLGame.core.nodes;

public sealed class Canva(INode2D node)
{
    private INode2D Node => node;

    private readonly Vector2f position = new(node.Position.X, node.Position.Y);

    public void Draw(RenderWindow window)
    {
        DrawSurface(window);
        DrawSprite(window);
        DrawAction(window);
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

        var sprite = Content.GetResource((Sprite2D)Node.Body.Sprite);
        sprite.Color = new(255, 255, 255, Convert.ToByte(Node.Opacity));
        sprite.Position = position;

        sprite.TextureRect = Node.Body.Metadata?.IsFlipped() switch
        {
            true => new(Global.RECT, 0, -Global.RECT, Global.RECT),
            _ => new(0, 0, Global.RECT, Global.RECT)
        };

        window.Draw(sprite);
    }

    public void DrawAction(RenderWindow window)
    {
        if (Is.Null(Node.Body) || Node.Body?.Health?.HasUpdate() is false) return;

        string damage = $"{Node.Body?.Health?.GetHealth()}";

        var text = new Text(damage, default, 18)
        {
            Position = position,
            FillColor = new Color(Color.Black),
            Font = Content.GetResource(Fonte.OpenSansSemibold),
        };
        window.Draw(text);
    }
    #endregion
}