namespace GameUI.Core.Interfaces;

public interface IHealthComponent
{
    bool HasUpdate();
    float GetHealth();
    float GetMaxHealth();
    void ReceiveTo(ref INode node, float value);
}

public interface IActionComponent
{
    void DamageTo(ref INode node, ref object? key);
}

public interface IMovementComponent
{
    void PushTo(ref INode node, ref object? key);
    void MoveTo(ref INode node, ref object? key);
}
