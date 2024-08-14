namespace GameUI.Core.Nodes;

public sealed class NodeGraphic : GraphicsView, INode2D
{
    public NodeGraphic()
    {
        Opacity = 0.0;
        Canva = new NodeCanva();
        Navigate = new NodeNavigation(this);
        WidthRequest = GameEnvironment.VECTOR;
        HeightRequest = GameEnvironment.VECTOR;
        Drawable = Canva;

        App.Subscribe(Event.LoadScene, Load);
    }

    #region Property
    private Tile2D _tile;
    private IBody2D? _body;
    private IShader? _shader;

    public NodeCanva Canva { get; }
    public NodeNavigation Navigate { get; }

    public Tile2D Tile { get => _tile; set { _tile = value; Draw(); } }
    public IBody2D? Body { get => _body; set { _body = value; Draw(); } }
    public IShader? Shader { get => _shader; set { _shader = value; Draw(); } }
    #endregion

    #region Build
    private void Load(object? args)
    {
        if (Is.Null(args) || Is.Not<ScenePackage>(args)) return;

        var (row, column) = Navigate.Position;

        _tile = ((ScenePackage)args!).Surface?.ElementAtOrDefault(row)?.ElementAtOrDefault(column) ?? default;

        Draw();
    }
    #endregion

    #region Action
    public void Draw()
    {
        Canva.Tile = Tile;
        Canva.Body = Body;
        Canva.Shader = Shader;
        Invalidate();
    }

    public async void FadeTo(double opacity)
    {
        await this.FadeTo(opacity, 150, Easing.Linear);
    }
    #endregion
}
