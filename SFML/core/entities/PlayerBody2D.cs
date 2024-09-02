namespace SFMLGame.core.entities;

public sealed class PlayerBody2D : IBody2D, IDisposable
{
    public PlayerBody2D(INode2D node)
    {
        Node = node;
        Sprite = Sprite2D.Aracne;
        Light = new LightComponent();
        Action = new ActionComponent();
        Health = new HealthComponent();
        Metadata = new MetadataComponent();
        Movement = new MovementComponent();

        Global.Subscribe(EEvent.KeyPressed, Execute);
    }

    #region Property
    public INode2D? Node { get; private set; }
    public Sprite2D? Sprite { get; private set; }
    public ILightComponent? Light { get; private set; }
    public IActionComponent? Action { get; private set; }
    public IHealthComponent? Health { get; private set; }
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
        Global.Invoke(EEvent.Camera, Node?.Position);
    }
    public void SetSprite(Sprite2D? sprite) => Sprite = sprite;
    public void SetBody(IBody2D? body) => Node?.SetBody(body);
    #endregion

    public void Dispose()
    {
        Light = null;
        Action = null;
        Health = null;
        Metadata = null;
        Movement = null;
        SetSprite(null);
        SetBody(null);
        SetNode(null);

        Global.UnSubscribe(EEvent.KeyPressed, Execute);
    }
}
