namespace GameUI.Core.Nodes;

public sealed class NodeGraphic : GraphicsView, INode
{
    public NodeGraphic()
    {
        Running = true;
        NodeCanva = new NodeCanva();
        NodeNavigation = new NodeNavigation(this);
        WidthRequest = GameEnvironment.VECTOR;
        HeightRequest = GameEnvironment.VECTOR;
        Drawable = NodeCanva;

        StartInteraction += OnSelected;

        App.Subscribe(Event.LoadResource, (_) => Dispatcher.DispatchAsync(Draw));
    }

    #region Property
    public ISprite? Sprite { get; set; }
    public IShader? Shader { get; set; }
    public NodeNavigation NodeNavigation { get; }
    public NodeCanva NodeCanva { get; }
    public bool Running { get; set; }
    #endregion

    #region Bindable Property
    public Tile Tile
    {
        get { return (Tile)GetValue(TileProperty); }
        set { SetValue(TileProperty, value); }
    }

    internal static readonly BindableProperty TileProperty = BindableProperty.Create(
        nameof(Tile), typeof(Tile), typeof(NodeGraphic), default(Tile),
        propertyChanged: (BindableObject tile, object old, object value) => tile.SetValue(TileProperty, (Tile)value));
    #endregion

    #region Event
    public void OnSelected(object? sender, TouchEventArgs e)
    {
        Sprite = new StaticSprite();
        Shader = new StaticShader();
    }
    #endregion

    #region Action
    public async Task Draw()
    {
        while (Running)
        {
            NodeCanva.Tile = Tile;
            NodeCanva.Sprite = Sprite;
            NodeCanva.Shader = Shader;
            Invalidate();
            await Task.Delay(AuxFunction.random.Next(1000, 3000));
        }
    }
    #endregion
}
