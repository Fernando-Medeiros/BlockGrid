namespace SFMLGame.core.entities;

public sealed class PlayerBody2D : IBody2D, IDisposable
{
    public PlayerBody2D(INode2D node)
    {
        Node = node;
        Sprite = ESprite.Aracne;
        Light = new LightComponent();
        Action = new ActionComponent();
        Status = new StatusComponent(this);
        Metadata = new MetadataComponent();
        Movement = new MovementComponent();

        Global.Subscribe(EEvent.KeyPressed, Execute);
    }

    #region Property
    public INode2D? Node { get; private set; }
    public ESprite? Sprite { get; private set; }
    public ILightComponent? Light { get; private set; }
    public IActionComponent? Action { get; private set; }
    public IStatusComponent? Status { get; private set; }
    public IMetadataComponent? Metadata { get; private set; }
    public IMovementComponent? Movement { get; private set; }
    #endregion

    #region Action
    public void Execute(object? keyCode)
    {
        if (App.CurrentScene != EScene.World) return;

        Light?.VisibilityTo(this, EOpacity.Regular);

        Metadata?.DirectionTo(keyCode);

        Movement?.MoveTo(this, keyCode);

        Light?.VisibilityTo(this, EOpacity.Light);

        Action?.DamageTo(this, keyCode);
    }

    public void SetNode(INode2D? node)
    {
        Node = node;
        Global.Invoke(EEvent.Camera, Node?.Position2D);
    }
    public void SetSprite(ESprite? sprite) => Sprite = sprite;
    public void SetBody(IBody2D? body) => Node?.SetBody(body);
    #endregion

    public void Dispose()
    {
        Light = null;
        Action = null;
        Status = null;
        Metadata = null;
        Movement = null;
        SetSprite(null);
        SetBody(null);
        SetNode(null);

        Global.UnSubscribe(EEvent.KeyPressed, Execute);
    }
}
