namespace SFMLGame.core.components;

public sealed class ActionComponent : IActionComponent
{
    public void DamageTo(IBody2D body, object? keyCode)
    {
        if (Key.Actions.Contains(keyCode) is false) return;

        INode2D? node = default;

        foreach (var position in Key.Movement)
        {
            node = body.Metadata?.ResolveDirection(body, position);

            if (node?.Body is not null) break;
        }

        if (node is null || node.Body is null) return;

        node.Body?.Status?.ReceiveDamage(1);
    }
}
