namespace GameUI.Core.Nodes;

public sealed class NodeCanva : IDrawable
{
    static NodeCanva()
    {
        App.Subscribe(Event.LoadResource, (_) => Running = true);
    }

    #region Linked
    private static bool Running { get; set; }
    #endregion

    #region Property
    public Tile2D Tile { get; set; }
    public IBody2D? Body { get; set; }
    public IShader? Shader { get; set; }
    #endregion

    public void Draw(ICanvas canvas, RectF rect)
    {
        if (Running is false) return;

        canvas.Antialias = true;
        DrawSurface(canvas, rect);
        DrawSprite(canvas, rect);
        DrawAction(canvas, rect);
        DrawShader(canvas, rect);
    }

    #region Layer
    public void DrawShader(ICanvas canvas, RectF rect)
    {
        if (Is.Null(Shader)) return;

        var image = App.ResourceContainer.GetResource(Shader.Sprite);
        image?.Draw(canvas, rect);
    }

    public void DrawAction(ICanvas canvas, RectF rect)
    {
        if (Is.Null(Body) || Body?.Health?.HasUpdate() is false) return;

        string damage = $"{Body?.Health?.GetHealth()}";

        canvas.FontSize = 18;
        canvas.FontColor = Colors.White;
        canvas.DrawString(damage, rect.Center.X, rect.Center.Y, HorizontalAlignment.Justified);
    }

    public void DrawSprite(ICanvas canvas, RectF rect)
    {
        if (Is.Null(Body?.Sprite)) return;

        var image = App.ResourceContainer.GetResource((Sprite2D)Body.Sprite);
        image?.Draw(canvas, rect);
    }

    public void DrawSurface(ICanvas canvas, RectF rect)
    {
        var image = App.ResourceContainer.GetResource(Tile);
        image?.Draw(canvas, rect);
    }
    #endregion
}