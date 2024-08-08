namespace GameUI.Core.Components;

public sealed class HealthComponent : IHealthComponent
{
    private bool _hasUpdate;
    private float _health = 10;
    private float _maxHealth = 10;

    public bool HasUpdate() => _hasUpdate;
    public float GetHealth() => _health;
    public float GetMaxHealth() => _maxHealth;

    public void ReceiveTo(ref INode node, float value)
    {
        _health -= value;
        _hasUpdate = true;
        node.ReDraw();

        if (_health <= 0) node.Sprite = null;
    }
}
