namespace MONOGame.Core.Interfaces;

public interface IBody2D
{
    INode2D? Node { get; }
    Sprite2D? Sprite { get; }
    IActionComponent? Action { get; }
    IHealthComponent? Health { get; }
    IMovementComponent? Movement { get; }

    void Dispose();
    void Execute(object? sender);
    public void SetSprite(Sprite2D? sprite);
    public void SetNode(INode2D? node);
    public void SetBody(IBody2D? body);
}