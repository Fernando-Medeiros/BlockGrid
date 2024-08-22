namespace MAUIGame.Core.Interfaces;

public interface IBody2D
{
    INode2D? Node { get; }
    Sprite2D? Sprite { get; }
    IActionComponent? Action { get; }
    IHealthComponent? Health { get; }
    IMovementComponent? Movement { get; }

    void Dispose();
    void Execute(object? args);
    public void SetSprite(Sprite2D? x);
    public void SetNode(INode2D? x);
    public void SetBody(IBody2D? x);
}