namespace GameUI.Core.Components;

public sealed class MovementComponent : IMovementComponent
{
    /// <summary>
    /// Empura o _sprite do proximo bloco para a mesma direção.
    /// </summary>
    /// <param name="node">Referência ao node atual.</param>
    /// <param name="key">Tecla pressionada tipo string.</param>
    public void PushTo(ref INode node, ref object? key)
    {
        INode? next = node.NodeNavigation.GetBy(ref key);

        if (next is null || next.Sprite == null) return;

        next.Sprite.Movement?.MoveTo(ref next, ref key);
    }

    /// <summary>
    /// Movimenta o _sprite atual para o proximo Node caso ele esteja vazio e a acessibilidade seja livre.
    /// </summary>
    /// <param name="node">Referência ao node atual.</param>
    /// <param name="key">Tecla pressionada tipo string.</param>
    public void MoveTo(ref INode node, ref object? key)
    {
        INode? next = node.NodeNavigation.GetBy(ref key);

        if (next is null || TileAccess.ItsBlocked(next.Tile) || next.Sprite != null) return;

        ISprite? selfSprite = node.Sprite;

        node.NodeNavigation.VisibilityTo(0.2);
        node.Sprite = null;
        node = next;
        node.Sprite = selfSprite;
        node.FadeTo(1.0);
        node.NodeNavigation.VisibilityTo(1.0);
    }
}
