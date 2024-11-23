namespace SFMLGame.core.entities;

public sealed class StaticBody2D : IBody2D, IDisposable
{
    public StaticBody2D(INode2D node)
    {
        Node = node;
        Node.SetBody(this);

        Source = EBody.Static;
        Image = ESprite.Spider;
        Status = new StatusComponent(this);
        Movement = new MovementComponent();
    }

    #region Property
    public EBody? Source { get; private set; }
    public INode2D? Node { get; private set; }
    public Enum? Image { get; private set; }
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
    public void SetSprite(Enum? sprite) => Image = sprite;
    public void SetBody(IBody2D? body) => Node?.SetBody(body);
    #endregion

    public void Dispose()
    {
        Source = null;
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
