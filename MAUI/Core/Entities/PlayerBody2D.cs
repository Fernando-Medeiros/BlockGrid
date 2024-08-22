namespace MAUIGame.Core.Entities;

public sealed class PlayerBody2D : IBody2D, IDisposable
{
    public PlayerBody2D(INode2D node)
    {
        node.FadeTo(1);

        Node = node;
        Sprite = Sprite2D.Aracne;

        Action = new ActionComponent();
        Health = new HealthComponent();
        Movement = new MovementComponent();

        App.Subscribe(Event.KeyDown, Execute);
    }

    #region Property
    public INode2D? Node { get; private set; }
    public Sprite2D? Sprite { get; private set; }
    public IActionComponent? Action { get; private set; }
    public IHealthComponent? Health { get; private set; }
    public IMovementComponent? Movement { get; private set; }
    #endregion

    #region Action
    public void Execute(object? key)
    {
        Movement?.PushTo(this, key);
        Movement?.MoveTo(this, key);
        Action?.DamageTo(this, key);

        App.Invoke(Event.Camera, Node?.Navigate.Position);
    }

    public void SetSprite(Sprite2D? x) => Sprite = x;
    public void SetNode(INode2D? x) => Node = x;
    public void SetBody(IBody2D? x) { if (Node is not null) Node.Body = x; }
    #endregion

    public void Dispose()
    {
        Action = null;
        Health = null;
        Movement = null;
        SetSprite(null);
        SetBody(null);
        SetNode(null);

        App.UnSubscribe(Event.KeyDown, Execute);
    }
}
