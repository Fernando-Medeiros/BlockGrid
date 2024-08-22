namespace SFMLGame.Core.Interfaces;

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

public interface IMovementComponent
{
    void PushTo(IBody2D body, object? keyCode);
    void MoveTo(IBody2D body, object? keyCode);
    void MoveBy(IBody2D body, INode2D node);
}
