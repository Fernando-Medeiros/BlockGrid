namespace SFMLGame.core.interfaces;

public interface IMetadataComponent
{
    EDirection GetDirection();
    void DirectionTo(object? keyCode);
    INode2D? ResolveDirection(IBody2D body, object? keyCode);
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
    void VisibilityTo(IBody2D body, EOpacity opacity);
}

public interface IMovementComponent
{
    void MoveTo(IBody2D body, object? keyCode);
}
