namespace SFMLGame.Core.Components;

public sealed class MovementComponent : IMovementComponent
{
    public void PushTo(IBody2D body, object? keyCode)
    {
        var node = body.Node?.Get(keyCode);

        if (Is.Null(node) || Is.Null(node?.Body)) return;

        node?.Body?.Movement?.MoveTo(node.Body, keyCode);
    }

    public void MoveTo(IBody2D body, object? keyCode)
    {
        var node = body.Node?.Get(keyCode);

        if (Is.Null(node) || Is.Type<IBody2D>(node?.Body) || Is.Blocked(node?.Surface)) return;

        MoveBy(body, node);
    }

    public void MoveBy(IBody2D body, INode2D? node)
    {
        if (Is.Null(node) || Is.Type<IBody2D>(node?.Body) || Is.Blocked(node?.Surface)) return;

        body.SetBody(null);
        body.SetNode(node);
        body.SetBody(body);
    }
}
