namespace GameUI.Core.Nodes;

public sealed class NodeGraphic : GraphicsView, INode
{
    public NodeGraphic()
    {
        Opacity = 0.0;
        NodeCanva = new NodeCanva();
        NodeNavigation = new NodeNavigation(this);
        WidthRequest = GameEnvironment.VECTOR;
        HeightRequest = GameEnvironment.VECTOR;
        Drawable = NodeCanva;

        App.Subscribe(Event.LoadScene, (args) =>
        {
            Unpack(args);
            ReDraw();
        });
    }

    #region Property
    private Tile _tile;
    private ISprite? _sprite;
    private IShader? _shader;

    public NodeCanva NodeCanva { get; }
    public NodeNavigation NodeNavigation { get; }
    public Tile Tile { get => _tile; set { _tile = value; ReDraw(); } }
    public ISprite? Sprite { get => _sprite; set { _sprite = value; ReDraw(); } }
    public IShader? Shader { get => _shader; set { _shader = value; ReDraw(); } }
    #endregion

    #region Build
    private void Unpack(object? args)
    {
        if (args is null) return;

        var package = (ScenePackage)args;
        var (row, column) = NodeNavigation.Position;

        _tile = package.Surface?.ElementAtOrDefault(row)?.ElementAtOrDefault(column) ?? default;
    }
    #endregion

    #region Action
    public void ReDraw()
    {
        NodeCanva.Tile = Tile;
        NodeCanva.Sprite = Sprite;
        NodeCanva.Shader = Shader;
        Invalidate();
    }

    public async void FadeTo(double opacity)
    {
        await this.FadeTo(opacity, 150, Easing.Linear);
    }
    #endregion
}
