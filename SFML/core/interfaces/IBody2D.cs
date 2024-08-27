namespace SFMLGame.Core.Interfaces;

public interface IBody2D
{
    INode2D? Node { get; }
    Sprite2D? Sprite { get; }
    ILightComponent? Light { get; }
    IActionComponent? Action { get; }
    IHealthComponent? Health { get; }
    IMovementComponent? Movement { get; }

    void Dispose();
    void Execute(object? sender);
    void SetBody(IBody2D? body);
    void SetNode(INode2D? node);
    void SetSprite(Sprite2D? sprite);
}