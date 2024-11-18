namespace SFMLGame.core.entities;

public sealed class PlayerBody2D : IBody2D, IDisposable
{
    public PlayerBody2D(INode2D node)
    {
        Node = node;
        Node.SetBody(this);

        Type = EBody.Player;
        Sprite = App.Player.Race;
        Light = new LightComponent();
        Action = new ActionComponent();
        Status = new StatusComponent(this);
        Metadata = new MetadataComponent();
        Movement = new MovementComponent();

        Global.Subscribe(EEvent.KeyPressed, Execute);
    }

    #region Property
    public EBody? Type { get; private set; }
    public INode2D? Node { get; private set; }
    public Enum? Sprite { get; private set; }
    public ILightComponent? Light { get; private set; }
    public IActionComponent? Action { get; private set; }
    public IStatusComponent? Status { get; private set; }
    public IMetadataComponent? Metadata { get; private set; }
    public IMovementComponent? Movement { get; private set; }
    #endregion

    #region Action
    public void Execute(object? keyCode)
    {
        Light?.VisibilityTo(this, EOpacity.Regular);

        Metadata?.DirectionTo(keyCode);

        Movement?.MoveTo(this, keyCode);

        Light?.VisibilityTo(this, EOpacity.Light);

        Action?.DamageTo(this, keyCode);
    }

    public void SetNode(INode2D? node)
    {
        Node = node;
        Global.Invoke(EEvent.CameraChanged, Node?.Position2D);
    }
    public void SetSprite(Enum? sprite) => Sprite = sprite;
    public void SetBody(IBody2D? body) => Node?.SetBody(body);
    #endregion

    public void Dispose()
    {
        Light?.VisibilityTo(this, EOpacity.Opaque);

        Type = null;
        Light = null;
        Action = null;
        Status = null;
        Metadata = null;
        Movement = null;
        SetSprite(null);
        SetBody(null);
        SetNode(null);

        Global.Unsubscribe(EEvent.KeyPressed, Execute);
    }
}
