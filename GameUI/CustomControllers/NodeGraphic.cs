namespace GameUI.CustomControllers;

#region Structure
public class Global
{
    public const byte VECTOR = 32;
    public const byte MAX_ROW = 24;
    public const byte MAX_COLUMN = 44;
}

public enum ETileTexture : byte
{
    ASCII,
    Color,
    Image,
    Mono,
}

public enum ETile : byte
{
    Ground,
    Grass,
    Road,
    Desert,
    Water,
}

public sealed record Position(byte Row, byte Column);

public interface INode
{
    public ETile Tile { get; set; }
    public NodeTree NodeTree { get; }
    public Position Position { get; }
    public NodeCanvas Canvas { get; }
    public bool Running { get; set; }
    public void Draw();
    public Task Update();
    public void OnSelected(object? sender, TouchEventArgs e);
}

public sealed class NodeTree
{
    public NodeGraphic? TopLeft { get; set; }
    public NodeGraphic? Top { get; set; }
    public NodeGraphic? TopRight { get; set; }
    public NodeGraphic? Left { get; set; }
    public NodeGraphic? Right { get; set; }
    public NodeGraphic? BottomLeft { get; set; }
    public NodeGraphic? Bottom { get; set; }
    public NodeGraphic? BottomRight { get; set; }
}
#endregion

public sealed class NodeGraphic : GraphicsView, INode
{
    public NodeGraphic()
    {
        Position = new(Row, Column);

        Nodes[Position.Row].Add(this);

        Column++;
        if (Column >= Global.MAX_COLUMN) Row++;
        if (Column >= Global.MAX_COLUMN) Column = 0;

        NodeTree = new NodeTree();
        Canvas = new NodeCanvas();

        Running = true;
        Opacity = 0.8;
        Drawable = Canvas;
        WidthRequest = Global.VECTOR;
        HeightRequest = Global.VECTOR;

        StartInteraction += OnSelected;

        Dispatcher.DispatchAsync(Update);
    }

    #region Linked
    private static byte Row;
    private static byte Column;
    private static readonly IList<IList<NodeGraphic>> Nodes = [[], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], []];
    #endregion

    #region Property
    public Position Position { get; }
    public NodeTree NodeTree { get; }
    public NodeCanvas Canvas { get; }
    public bool Running { get; set; }
    #endregion

    #region Bindable Property
    public ETile Tile
    {
        get { return (ETile)GetValue(TileProperty); }
        set { SetValue(TileProperty, value); }
    }

    internal static readonly BindableProperty TileProperty = BindableProperty.Create(
        nameof(Tile), typeof(ETile), typeof(NodeGraphic), default(ETile),
        propertyChanged: (BindableObject tile, object old, object value) => tile.SetValue(TileProperty, (ETile)value));
    #endregion

    #region Event
    public void OnSelected(object? sender, TouchEventArgs e)
    {
        NodeTree.TopRight?.FadeTo(0.82);
        NodeTree.TopLeft?.FadeTo(0.85);
        NodeTree.Top?.FadeTo(0.9);
        NodeTree.Left?.FadeTo(0.95);
        this.FadeTo(1);
        NodeTree.Right?.FadeTo(0.95);
        NodeTree.Bottom?.FadeTo(0.9);
        NodeTree.BottomLeft?.FadeTo(0.85);
        NodeTree.BottomRight?.FadeTo(0.82);
    }
    #endregion

    #region Action
    public async Task Update()
    {
        await Task.Delay(5000);

        FindNodeTree();

        while (Running)
        {
            Draw();
            Invalidate();
            await Task.Delay(AuxFun.random.Next(3000));
        }
    }

    public void Draw()
    {
        Canvas.Tile = Tile;
    }

    public void FindNodeTree()
    {
        var (row, column) = Position;

        NodeTree.Left = Nodes.ElementAtOrDefault(row)?.ElementAtOrDefault(column - 1);
        NodeTree.Right = Nodes.ElementAtOrDefault(row)?.ElementAtOrDefault(column + 1);

        NodeTree.Top = Nodes.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column);
        NodeTree.TopLeft = Nodes.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column - 1);
        NodeTree.TopRight = Nodes.ElementAtOrDefault(row - 1)?.ElementAtOrDefault(column + 1);

        NodeTree.Bottom = Nodes.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column);
        NodeTree.BottomLeft = Nodes.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column - 1);
        NodeTree.BottomRight = Nodes.ElementAtOrDefault(row + 1)?.ElementAtOrDefault(column + 1);
    }
    #endregion
}

public sealed class NodeCanvas : IDrawable
{
    #region Linked
    public static ETileTexture Texture { get; set; }
    #endregion

    #region Property
    public ETile Tile { get; set; }
    #endregion

    public void Draw(ICanvas canvas, RectF rect)
    {
        if (App.ResourceContainer.Available is false) return;

        canvas.Antialias = true;
        DrawSurface(canvas, rect);
        DrawObject(canvas, rect);
    }

    #region Layer
    public void DrawObject(ICanvas canvas, RectF rect)
    {
    }

    public void DrawSurface(ICanvas canvas, RectF rect)
    {
        var resource = App.ResourceContainer.GetResource(Texture, Tile);

        if (Texture is ETileTexture.ASCII)
        {
            var resourceColor = App.ResourceContainer.GetResource(ETileTexture.Color, Tile);

            canvas.FontColor = (GraphicsColor)resourceColor;
            canvas.DrawString((string)resource, rect.Center.X, rect.Center.Y, HorizontalAlignment.Justified);
        }

        if (Texture is ETileTexture.Image)
        {
            canvas.DrawImage((GraphicsImage)resource, rect.X, rect.Y, rect.Width, rect.Height);
        }

        if (Texture is ETileTexture.Color or ETileTexture.Mono)
        {
            canvas.FillColor = (GraphicsColor)resource;
            canvas.FillRoundedRectangle(rect, 2f);
        }
    }
    #endregion
}
