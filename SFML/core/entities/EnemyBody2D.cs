namespace SFMLGame.core.entities;

public sealed class EnemyBody2D : IBody2D, IDisposable
{
    public EnemyBody2D(INode2D node)
    {
        Node = node;
        Sprite = ESprite.Spider;
        Status = new StatusComponent(this);
        Movement = new MovementComponent();
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
    }

    public void SetNode(INode2D? node) => Node = node;
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
    }
}
