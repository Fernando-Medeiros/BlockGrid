namespace MONOGame.Core.Nodes;

public sealed class Canva(Position2D position2D)
{
    private Vector2 Position { get; } = new(position2D.PosX, position2D.PosY);

    public IBody2D? Body { get; set; }
    public Surface2D Surface { get; set; }

    public void Draw(SpriteBatch window)
    {
        DrawSurface(window);
        DrawSprite(window);
        DrawAction(window);
    }

    #region Layer
    private void DrawSurface(SpriteBatch window)
    {
        var texture = App.Resources.GetResource(Surface);
        window.Draw(texture, Position, Color.White);
    }

    public void DrawAction(SpriteBatch window)
    {
        if (Is.Null(Body) || Body?.Health?.HasUpdate() is false) return;

        string damage = $"{Body?.Health?.GetHealth()}";

        var font = App.Resources.GetResource(Fonte.OpenSansSemibold);

        window.DrawString(font, damage, Position, Color.Red);
    }

    public void DrawSprite(SpriteBatch window)
    {
        if (Is.Null(Body?.Sprite)) return;

        var texture = App.Resources.GetResource((Sprite2D)Body.Sprite);
        window.Draw(texture, Position, Color.White);
    }
    #endregion
}