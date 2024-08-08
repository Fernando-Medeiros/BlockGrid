namespace GameUI.Core.Components;

public sealed class ActionComponent : IActionComponent
{
    public void DamageTo(ref INode node, ref object? key)
    {
        if (Key.Actions.Contains(key) is false) return;

        INode? next = default;

        foreach (object pos in Key.Positions)
        {
            var positionKey = pos;
            next = node.NodeNavigation.GetBy(ref positionKey);

            if (next?.Sprite is not null) break;
        }

        if (next is null || next.Sprite is null || TileAccess.ItsBlocked(next.Tile)) return;

        next.Sprite?.Health?.ReceiveTo(ref next, 1);
    }
}
