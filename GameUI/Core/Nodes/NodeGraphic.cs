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

        App.Subscribe(Event.LoadScene, (_) => ReDraw());
    }

    #region Property
    private ISprite? sprite;
    private IShader? shader;

    public NodeCanva NodeCanva { get; }
    public NodeNavigation NodeNavigation { get; }
    public ISprite? Sprite { get => sprite; set { sprite = value; ReDraw(); } }
    public IShader? Shader { get => shader; set { shader = value; ReDraw(); } }
    public Tile Tile { get => (Tile)GetValue(TileProperty); set { SetValue(TileProperty, value); } }
    #endregion

    #region Bindable Property
    internal static readonly BindableProperty TileProperty = BindableProperty.Create(
        nameof(Tile), typeof(Tile), typeof(NodeGraphic), default(Tile),
        propertyChanged: (BindableObject tile, object old, object value) => tile.SetValue(TileProperty, (Tile)value));
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
