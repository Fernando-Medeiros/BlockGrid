namespace SFMLGame.core.entities;

public sealed class EnemyBody2D : IBody2D, IDisposable
{
    public EnemyBody2D(INode2D node)
    {
        Node = node;
        Node.SetBody(this);

        Type = EBody.Enemy;
        Sprite = enums.ESprite.Spider;
        Status = new StatusComponent(this);
        Movement = new MovementComponent();
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
    }

    public void SetNode(INode2D? node) => Node = node;
    public void SetSprite(Enum? sprite) => Sprite = sprite;
    public void SetBody(IBody2D? body) => Node?.SetBody(body);
    #endregion

    public void Dispose()
    {
        Type = null;
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
