namespace SFMLGame.Core.Nodes;

public sealed class Canva(Position2D position2D)
{
    private Vector2f Position { get; } = new(position2D.PosX, position2D.PosY);

    public Opacity Opacity { get; set; }
    public IBody2D? Body { get; set; }
    public Surface2D Surface { get; set; }

    public void Draw(RenderWindow window)
    {
        DrawSurface(window);
        DrawSprite(window);
        DrawAction(window);
    }

    #region Layer
    private void DrawSurface(RenderWindow window)
    {
        var sprite = App.Resources.GetResource(Surface);
        sprite.Position = Position;
        sprite.Color = new(255, 255, 255, Convert.ToByte(Opacity));
        window.Draw(sprite);
    }

    public void DrawSprite(RenderWindow window)
    {
        if (Body?.Sprite is null) return;

        var sprite = App.Resources.GetResource((Sprite2D)Body.Sprite);
        sprite.Position = Position;
        sprite.Color = new(255, 255, 255, Convert.ToByte(Opacity));
        window.Draw(sprite);
    }

    public void DrawAction(RenderWindow window)
    {
        if (Is.Null(Body) || Body?.Health?.HasUpdate() is false) return;

        string damage = $"{Body?.Health?.GetHealth()}";

        var text = new Text(damage, default, 18)
        {

            Position = Position,
            FillColor = new Color(Color.Black),
            Font = App.Resources.GetResource(Fonte.OpenSansSemibold),
        };
        window.Draw(text);
    }
    #endregion
}