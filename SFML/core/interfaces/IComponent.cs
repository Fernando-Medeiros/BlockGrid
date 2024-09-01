namespace SFMLGame.core.interfaces;

public interface IMetadataComponent
{
    bool IsFlipped();
    void FlipTo(object? keyCode);
}

public interface IHealthComponent
{
    bool HasUpdate();
    float GetHealth();
    float GetMaxHealth();
    void ReceiveTo(IBody2D body, float value);
}

public interface IActionComponent
{
    void DamageTo(IBody2D body, object? keyCode);
}

public interface ILightComponent
{
    void VisibilityTo(INode2D node, EOpacity opacity);
}

public interface IMovementComponent
{
    void PushTo(IBody2D body, object? keyCode);
    void MoveTo(IBody2D body, object? keyCode);
    void MoveBy(IBody2D body, INode2D node);
}
