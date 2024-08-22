namespace SFMLGame.Core.Entities;

public sealed class PlayerBody2D : IBody2D, IDisposable
{
    public PlayerBody2D(INode2D node)
    {
        Node = node;
        Sprite = Sprite2D.Aracne;
        Action = new ActionComponent();
        Health = new HealthComponent();
        Movement = new MovementComponent();

        App.Global.Subscribe(CoreEvent.KeyPressed, Execute);
    }

    #region Property
    public INode2D? Node { get; private set; }
    public Sprite2D? Sprite { get; private set; }
    public IActionComponent? Action { get; private set; }
    public IHealthComponent? Health { get; private set; }
    public IMovementComponent? Movement { get; private set; }
    #endregion

    #region Action
    public void Execute(object? keyCode)
    {
        Movement?.PushTo(this, keyCode);
        Movement?.MoveTo(this, keyCode);
        Action?.DamageTo(this, keyCode);

        App.Global.Invoke(CoreEvent.Camera, Node?.Position);
    }

    public void SetSprite(Sprite2D? sprite) => Sprite = sprite;
    public void SetNode(INode2D? node) => Node = node;
    public void SetBody(IBody2D? body) { if (Node is not null) Node.Body = body; }
    #endregion

    public void Dispose()
    {
        Action = null;
        Health = null;
        Movement = null;
        SetSprite(null);
        SetBody(null);
        SetNode(null);

        App.Global.UnSubscribe(CoreEvent.KeyPressed, Execute);
    }
}
