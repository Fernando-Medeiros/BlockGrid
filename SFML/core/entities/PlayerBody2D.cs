namespace SFMLGame.Core.Entities;

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

        Light.VisibilityTo(Node, Opacity.Light);

        App.Global.Subscribe(CoreEvent.KeyPressed, Execute);
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
        Metadata?.FlipTo(keyCode);

        Movement?.PushTo(this, keyCode);

        Light?.VisibilityTo(Node, Opacity.Regular);

        Movement?.MoveTo(this, keyCode);

        Light?.VisibilityTo(Node, Opacity.Light);

        Action?.DamageTo(this, keyCode);

        App.Global.Invoke(CoreEvent.Camera, Node?.Position);
    }

    public void SetNode(INode2D? node) => Node = node;
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

        App.Global.UnSubscribe(CoreEvent.KeyPressed, Execute);
    }
}
