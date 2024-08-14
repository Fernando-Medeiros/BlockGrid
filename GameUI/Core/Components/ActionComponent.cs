namespace GameUI.Core.Components;

public sealed class ActionComponent : IActionComponent
{
    public void DamageTo(IBody2D body, object? key)
    {
        if (Key.Actions.Contains(key) is false) return;

        INode2D? node = default;

        foreach (var position in Key.Positions)
        {
            node = body.Node?.Navigate.GetNode(position);

            if (Is.Type<IBody2D>(node?.Body)) break;
        }

        if (Is.Null(node) || Is.Null(node?.Body) || Is.Blocked(node?.Tile)) return;

        node?.Body?.Health?.ReceiveTo(node.Body, 1);
    }
}
