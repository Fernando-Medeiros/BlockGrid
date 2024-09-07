namespace SFMLGame.core.interfaces;

public interface IBody2D
{
    INode2D? Node { get; }
    ESprite? Sprite { get; }
    ILightComponent? Light { get; }
    IStatusComponent? Status { get; }
    IActionComponent? Action { get; }
    IMetadataComponent? Metadata { get; }
    IMovementComponent? Movement { get; }

    void Dispose();
    void Execute(object? sender);
    void SetBody(IBody2D? body);
    void SetNode(INode2D? node);
    void SetSprite(ESprite? sprite);
}