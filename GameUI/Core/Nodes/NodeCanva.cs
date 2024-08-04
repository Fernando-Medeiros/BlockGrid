namespace GameUI.Core.Nodes;

public sealed class NodeCanva : IDrawable
{
    static NodeCanva()
    {
        App.Subscribe(Event.LoadResource, (_) => Running = true);
        App.Subscribe(Event.TileTexture, (args) => Texture = (TileTexture)args!);
    }

    #region Linked
    private static bool Running { get; set; }
    private static TileTexture Texture { get; set; }
    #endregion

    #region Property
    public Tile Tile { get; set; }
    public ISprite? Sprite { get; set; }
    public IShader? Shader { get; set; }
    #endregion

    public void Draw(ICanvas canvas, RectF rect)
    {
        if (Running is false) return;

        canvas.Antialias = true;
        DrawSurface(canvas, rect);
        DrawSprite(canvas, rect);
        DrawShader(canvas, rect);
    }

    #region Layer
    public void DrawShader(ICanvas canvas, RectF rect)
    {
        if (Shader is null) return;

        var resource = App.ResourceContainer.GetResource(Shader.Image);
        canvas.DrawImage((GraphicsImage)resource, rect.X, rect.Y, rect.Width, rect.Height);
    }

    public void DrawSprite(ICanvas canvas, RectF rect)
    {
        if (Sprite is null) return;

        var resource = App.ResourceContainer.GetResource(Sprite.Image);
        canvas.DrawImage((GraphicsImage)resource, rect.X, rect.Y, rect.Width, rect.Height);
    }

    public void DrawSurface(ICanvas canvas, RectF rect)
    {
        var resource = App.ResourceContainer.GetResource(Texture, Tile);

        if (Texture is TileTexture.ASCII)
        {
            var resourceColor = App.ResourceContainer.GetResource(TileTexture.Color, Tile);

            canvas.FontColor = (GraphicsColor)resourceColor;
            canvas.DrawString((string)resource, rect.Center.X, rect.Center.Y, HorizontalAlignment.Justified);
        }

        if (Texture is TileTexture.Image)
        {
            canvas.DrawImage((GraphicsImage)resource, rect.X, rect.Y, rect.Width, rect.Height);
        }

        if (Texture is TileTexture.Color)
        {
            canvas.FillColor = (GraphicsColor)resource;
            canvas.FillRoundedRectangle(rect, 2f);
        }
    }
    #endregion
}